﻿using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;
using EFKIS.Entities;
using EFKIS.Concrete;
//using EFKIS.Helpers;
using System.Globalization;
using EFRW.Entities1;
using RW;
using EFRW.Concrete;
using EFMT.Concrete;
using EFKIS.Abstract;
using EFMT.Entities;

namespace KIS
{
    public class KIS_RW_Transfer : KISTransfer
    {
        private eventID eventID = eventID.KIS_RWTransfer;
        //TODO: Сдесь отключаем создание справочников из данных КИС
        private bool reference_kis = true; // Использовать справочники КИС

        //protected service servece_owner = service.Null;
        bool log_detali = true;

        public KIS_RW_Transfer()
            : base()
        {

        }

        public KIS_RW_Transfer(service servece_owner)
            : base(servece_owner)
        {
            this.servece_owner = servece_owner;
        }

        #region Таблица переноса составов из КИС [RWBufferArrivalSostav]
        /// <summary>
        /// Сохранить состав из КИС
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        protected int SaveRWBufferArrivalSostav(Prom_Sostav ps, statusSting status)
        {
            EFTKIS ef_tkis = new EFTKIS();
            try
            {
                // Проверка правилности заполнения времени и всех полей натурки
                if (ps.D_DD == null || ps.D_MM == null || ps.D_YY == null || ps.T_HH == null || ps.T_MI == null)
                {
                    String.Format("[RailWay] Ошибка заполнения натурного листа {0}, невозможно определить дату и время: {1}-{2}-{3} {4}:{5}. Строка не добавлена в таблицу состояния переноса составов зашедших на АМКР по данным системы КИС", ps.N_NATUR, ps.D_DD, ps.D_MM, ps.D_YY, ps.T_HH, ps.T_MI).WriteWarning(servece_owner, this.eventID);
                    return 0;
                }
                // p_ot=0, k_st_otpr is not null,  k_st_pr is null
                if (ps.P_OT == 0 && ps.K_ST_OTPR != null && ps.K_ST_PR == null)
                {
                    return ef_tkis.SaveRWBufferArrivalSostav(new RWBufferArrivalSostav()
                    {
                        id = 0,
                        datetime = (DateTime)ps.DT,
                        day = (int)ps.D_DD,
                        month = (int)ps.D_MM,
                        year = (int)ps.D_YY,
                        hour = (int)ps.T_HH,
                        minute = (int)ps.T_MI,
                        natur = ps.N_NATUR,
                        id_station_kis = (int)ps.K_ST,
                        way_num = ps.N_PUT,
                        napr = ps.NAPR,
                        count_wagons = null,
                        count_nathist = null,
                        count_set_wagons = null,
                        count_set_nathist = null,
                        close = null,
                        close_user = null,
                        status = (int)status,
                        list_wagons = null,
                        list_no_set_wagons = null,
                        list_no_update_wagons = null,
                        message = null,
                    });
                }
                else
                {
                    String.Format("Натурный листа {0}, за: {1}-{2}-{3} {4}:{5} - не соответствует критерию заполнения P_OT={6} (0), K_ST_OTPR={7} (is not null), K_ST_PR={8} (is null). Строка не добавлена в таблицу состояния переноса составов сданных на УЗ по данным системы КИС",
                        ps.N_NATUR, ps.D_DD, ps.D_MM, ps.D_YY, ps.T_HH, ps.T_MI, ps.P_OT, ps.K_ST_OTPR, ps.K_ST_PR).WriteWarning(servece_owner, this.eventID);
                    return 0;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveRWBufferArrivalSostav(ps={0}, status={1})", ps.GetFieldsAndValue(), status), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Найти и удалить из списка ArrivalSostav елемент natur
        /// </summary>
        /// <param name="list"></param>
        /// <param name="natur"></param>
        /// <returns></returns>
        protected bool DelExistRWBufferArrivalSostav(ref List<RWBufferArrivalSostav> list, int natur)
        {
            bool Result = false;
            try
            {
                int index = list.Count() - 1;
                while (index >= 0)
                {
                    if (list[index].natur == natur)
                    {
                        list.RemoveAt(index);
                        Result = true;
                    }
                    index--;
                }
                return Result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DelExistRWBufferArrivalSostav(list={0}, natur={1})", list, natur), servece_owner, eventID);
            }
            return Result;
        }
        /// <summary>
        /// Проверяет списки PromSostav и ArrivalSostav на повторяющие натурные листы, оставляет в списке PromSostav - добавленные составы, ArrivalSostav - удаленные из КИС составы
        /// </summary>
        /// <param name="list_ps"></param>
        /// <param name="list_as"></param>
        protected void DelExistRWBufferArrivalSostav(ref List<Prom_Sostav> list_ps, ref List<RWBufferArrivalSostav> list_as)
        {
            try
            {
                int index = list_ps.Count() - 1;
                while (index >= 0)
                {
                    if (DelExistRWBufferArrivalSostav(ref list_as, list_ps[index].N_NATUR))
                    {
                        list_ps.RemoveAt(index);
                    }
                    index--;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DelExistRWBufferArrivalSostav(list_ps={0}, list_as={1})", list_ps, list_as), servece_owner, eventID);
            }
        }
        /// <summary>
        /// Удалить ранее перенесеные составы
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected int DeleteRWBufferArrivalSostav(List<RWBufferArrivalSostav> list)
        {
            EFTKIS ef_tkis = new EFTKIS();
            try
            {
                if (list == null | list.Count == 0) return 0;
                int delete = 0;
                int errors = 0;
                foreach (RWBufferArrivalSostav or_as in list)
                {
                    // Удалим вагоны из системы RailCars
                    // TODO: Сделать код удаления вагонов из RailWay
                    //transfer_rc.DeleteVagonsToNaturList(or_as.NaturNum, or_as.DateTime);
                    or_as.close = DateTime.Now;
                    or_as.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                    or_as.status = (int)statusSting.Delete;
                    int res = ef_tkis.SaveRWBufferArrivalSostav(or_as);
                    if (res > 0) { 
                        delete++;
                        if (log_detali)
                        {
                            String.Format("[RailWay] Таблица состояния переноса составов зашедших на АМКР по данным системы КИС - удален составов:(натурка: {0}, дата: {1}, ID:{2}.", or_as.natur, or_as.datetime, or_as.id).WriteWarning(servece_owner, this.eventID);
                        }
                    } 
                    if (res < 0)
                    {
                        String.Format("[RailWay] Ошибка выполнения метода DeleteArrivalSostav, удаление строки:{0} из таблицы состояния переноса составов зашедших на АМКР по данным системы КИС", or_as.id).WriteError(servece_owner, this.eventID);
                        errors++;
                    }
                }
                String.Format("[RailWay] Таблица состояния переноса составов зашедших на АМКР по данным системы КИС, определенно удаленных в системе КИС {0} составов, удалено из таблицы {1}, ошибок удаления {2}.", list.Count(), delete, errors).WriteWarning(servece_owner, this.eventID);
                return delete;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteRWBufferArrivalSostav(list={0})", list), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Добавить новые составы появившиеся после переноса
        /// </summary>
        /// <param name="list"></param>
        protected int InsertRWBufferArrivalSostav(List<Prom_Sostav> list)
        {
            try
            {
                if (list == null | list.Count == 0) return 0;
                int insers = 0;
                int errors = 0;
                foreach (Prom_Sostav ps in list)
                {
                    int res = SaveRWBufferArrivalSostav(ps, statusSting.Insert);
                    if (res > 0) insers++;
                    if (res < 0)
                    {
                        String.Format("[RailWay] Ошибка выполнения метода InsertArrivalSostav, добавления строки состава по данным системы КИС(натурный лист:{0}, дата:{1}-{2}-{3} {4}:{5}) в таблицу состояния переноса составов зашедших на АМКР по данным системы КИС", ps.N_NATUR, ps.D_DD, ps.D_MM, ps.D_YY, ps.T_HH, ps.T_MI).WriteError(servece_owner, this.eventID);
                        errors++;
                    }
                }
                String.Format("[RailWay] Таблица состояния переноса составов зашедших на АМКР по данным системы КИС, определенно добавленных в системе КИС {0} составов, добавлено в таблицу {1}, ошибок добавления {2}.", list.Count(), insers, errors).WriteWarning(servece_owner, this.eventID);
                return insers;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("InsertRWBufferArrivalSostav(list={0})", list), servece_owner, eventID);
                return -1;
            }
        }
        #endregion

        #region Таблица переноса составов из КИС [RWBufferSendingSostav]
        /// <summary>
        /// Сохранить состав из КИС
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        protected int SaveRWBufferSendingSostav(Prom_Sostav ps, statusSting status)
        {
            EFTKIS ef_tkis = new EFTKIS();
            try
            {
                // Проверка правилности заполнения времени и всех полей натурки
                if (ps.D_DD == null || ps.D_MM == null || ps.D_YY == null || ps.T_HH == null || ps.T_MI == null) {
                    String.Format("[RailWay] Ошибка заполнения натурного листа {0}, невозможно определить дату и время: {1}-{2}-{3} {4}:{5}. Строка не добавлена в таблицу состояния переноса составов сданных на УЗ по данным системы КИС", ps.N_NATUR, ps.D_DD, ps.D_MM, ps.D_YY, ps.T_HH, ps.T_MI).WriteWarning(servece_owner, this.eventID);
                    return 0;
                }
                // p_ot=1, n_ved_pr is null, k_st_otpr is null , k_st_pr is not null
                if (ps.P_OT == 1 && ps.N_VED_PR == null && ps.K_ST_OTPR == null && ps.K_ST_PR != null)
                {
                    return ef_tkis.SaveRWBufferSendingSostav(new RWBufferSendingSostav()
                    {
                        id = 0,
                        datetime = (DateTime)ps.DT,
                        day = (int)ps.D_DD,
                        month = (int)ps.D_MM,
                        year = (int)ps.D_YY,
                        hour = (int)ps.T_HH,
                        minute = (int)ps.T_MI,
                        natur = ps.N_NATUR,
                        id_station_from_kis = (int)ps.K_ST,
                        id_station_on_kis = (int)ps.K_ST_PR,
                        count_nathist = null,
                        count_set_nathist = null,
                        close = null,
                        close_user = null,
                        status = (int)status,
                        list_no_set_wagons = null,
                        message = null,
                    });
                }
                else
                {
                    String.Format("[RailWay] Натурный листа {0}, за: {1}-{2}-{3} {4}:{5} - не соответствует критерию заполнения P_OT={6} (1), N_VED_PR={7} (is null), K_ST_OTPR={8} (is null), K_ST_PR={9} (is not null). Строка не добавлена в таблицу состояния переноса составов сданных на УЗ по данным системы КИС",
                        ps.N_NATUR, ps.D_DD, ps.D_MM, ps.D_YY, ps.T_HH, ps.T_MI, ps.P_OT, ps.N_VED_PR, ps.K_ST_OTPR, ps.K_ST_PR).WriteWarning(servece_owner, this.eventID);
                    return 0;
                }

            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveRWBufferSendingSostav(ps={0}, status={1})", ps.GetFieldsAndValue(), status), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Найти и удалить из списка ArrivalSostav елемент natur
        /// </summary>
        /// <param name="list"></param>
        /// <param name="natur"></param>
        /// <returns></returns>
        protected bool DelExistRWBufferSendingSostav(ref List<RWBufferSendingSostav> list, int natur)
        {
            bool Result = false;
            try
            {
                int index = list.Count() - 1;
                while (index >= 0)
                {
                    if (list[index].natur == natur)
                    {
                        list.RemoveAt(index);
                        Result = true;
                    }
                    index--;
                }
                return Result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DelExistRWBufferSendingSostav(list={0}, natur={1})", list, natur), servece_owner, eventID);
            }
            return Result;
        }
        /// <summary>
        /// Проверяет списки PromSostav и ArrivalSostav на повторяющие натурные листы, оставляет в списке PromSostav - добавленные составы, ArrivalSostav - удаленные из КИС составы
        /// </summary>
        /// <param name="list_ps"></param>
        /// <param name="list_as"></param>
        protected void DelExistRWBufferSendingSostav(ref List<Prom_Sostav> list_ps, ref List<RWBufferSendingSostav> list_as)
        {
            try
            {
                List<int> list_natur = new List<int>();
                int index = list_ps.Count() - 1;
                while (index >= 0)
                {
                    // Проверка на повторяющуюся натурку
                    int? search = null;
                    foreach (int natur in list_natur)
                    {
                        if (natur == list_ps[index].N_NATUR)
                        { search = list_ps[index].N_NATUR; } // Уже была, надо удалить
                    }
                    if (search == null)
                    {
                        // Натурка не повторяется. Отправить на сравнение
                        list_natur.Add(list_ps[index].N_NATUR);
                        if (DelExistRWBufferSendingSostav(ref list_as, list_ps[index].N_NATUR))
                        {
                            list_ps.RemoveAt(index);
                        }
                    }
                    else
                    {   // Eсть повторение. Удалить из спсиска вставки новых натурок
                        list_ps.RemoveAt(index);
                        String.Format("[RailWay] Таблица состояния переноса составов зданных на УЗ по данным системы КИС, определенна повторяющаяся натурка {0}.", list_ps[index].N_NATUR).WriteWarning(servece_owner, this.eventID);
                    }
                    index--;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DelExistRWBufferSendingSostav(list_ps={0}, list_as={1})", list_ps, list_as), servece_owner, eventID);
            }
        }
        /// <summary>
        /// Удалить ранее перенесеные составы
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected int DeleteRWBufferSendingSostav(List<RWBufferSendingSostav> list)
        {
            EFTKIS ef_tkis = new EFTKIS();
            try
            {
                if (list == null | list.Count == 0) return 0;
                int delete = 0;
                int errors = 0;
                foreach (RWBufferSendingSostav bss in list)
                {
                    // TODO: Сделать код удаления вагонов из RailWay
                    //transfer_rc.DeleteVagonsToNaturList(or_as.NaturNum, or_as.DateTime);
                    bss.close = DateTime.Now;
                    bss.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                    bss.status = (int)statusSting.Delete;
                    int res = ef_tkis.SaveRWBufferSendingSostav(bss);
                    if (res > 0) delete++;
                    if (res < 0)
                    {
                        String.Format("[RailWay] Ошибка выполнения метода DeleteRWBufferSendingSostav, удаление строки:{0} из таблицы состояния переноса составов сданных на УЗ по данным системы КИС", bss.id).WriteError(servece_owner, this.eventID);
                        errors++;
                    }
                }
                String.Format("[RailWay] Таблица состояния переноса составов зданных на УЗ по данным системы КИС, определенно удаленных в системе КИС {0} составов, удалено из таблицы {1}, ошибок удаления {2}.", list.Count(), delete, errors).WriteWarning(servece_owner, this.eventID);
                return delete;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteRWBufferSendingSostav(list={0})", list), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Добавить новые составы появившиеся после переноса
        /// </summary>
        /// <param name="list"></param>
        protected int InsertRWBufferSendingSostav(List<Prom_Sostav> list)
        {
            try
            {
                if (list == null | list.Count == 0) return 0;
                int insers = 0;
                int errors = 0;
                foreach (Prom_Sostav ps in list)
                {
                    int res = SaveRWBufferSendingSostav(ps, statusSting.Insert);
                    if (res > 0) insers++;
                    if (res < 0)
                    {
                        String.Format("[RailWay] Ошибка выполнения метода InsertRWBufferSendingSostav, добавления строки состава по данным системы КИС(натурный лист:{0}, дата:{1}-{2}-{3} {4}:{5}) в таблицу состояния переноса составов сданных на УЗ по данным системы КИС", ps.N_NATUR, ps.D_DD, ps.D_MM, ps.D_YY, ps.T_HH, ps.T_MI).WriteError(servece_owner, this.eventID);
                        errors++;
                    }
                }
                String.Format("[RailWay] Таблица состояния переноса составов сданных на УЗ по данным системы КИС, определенно добавленных в системе КИС {0} составов, добавлено в таблицу {1}, ошибок добавления {2}.", list.Count(), insers, errors).WriteWarning(servece_owner, this.eventID);
                return insers;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("InsertRWBufferSendingSostav(list={0})", list), servece_owner, eventID);
                return -1;
            }
        }
        #endregion

        #region Таблица переноса составов из КИС [RWBufferInputSostav]
        /// <summary>
        /// Сохранить состав из КИС
        /// </summary>
        /// <param name="inp_sostav"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        protected int SaveRWBufferInputSostav(NumVag_Stpr1InStDoc inp_sostav, statusSting status)
        {
            try
            {
                EFTKIS ef_tkis = new EFTKIS();
                EFWagons ef_wag = new EFWagons();
                // Получим список вагонов
                List<NumVag_Stpr1InStVag> list_car = ef_wag.GetNumVag_Stpr1InStVag(inp_sostav.ID_DOC, inp_sostav.NAPR_IN_ST == 2 ? true : false).ToList();

                return ef_tkis.SaveRWBufferInputSostav(new RWBufferInputSostav()
                {
                    id = 0,
                    datetime = inp_sostav.DATE_IN_ST,
                    doc_num = inp_sostav.ID_DOC,
                    id_station_from_kis = inp_sostav.ST_IN_ST != null ? (int)inp_sostav.ST_IN_ST : 0,
                    way_num_kis = inp_sostav.N_PUT_IN_ST != null ? (int)inp_sostav.N_PUT_IN_ST : 0,
                    napr = inp_sostav.NAPR_IN_ST != null ? (int)inp_sostav.NAPR_IN_ST : 0,
                    id_station_on_kis = inp_sostav.K_STAN != null ? (int)inp_sostav.K_STAN : 0,
                    natur = inp_sostav.OLD_N_NATUR,
                    count_wagons = list_car != null ? (int?)list_car.Count() : null, // Определим количество вагонов
                    count_set_wagons = null,
                    close = null,
                    close_user = null,
                    close_comment = null, 
                    status = (int)status,
                    list_wagons = list_car != null ? GetWagonsToString(list_car.Select(v => v.N_VAG).ToArray().ToList()) : null, // Определим список вагонов 
                    list_no_set_wagons = null,
                    message = null, 
                });
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveRWBufferInputSostav(inp_sostav={0}, status={1})", inp_sostav.GetFieldsAndValue(), status), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Обновить список вагонов
        /// </summary>
        /// <param name="bis"></param>
        /// <returns></returns>
        public int UpdateCountWagons(ref RWBufferInputSostav bis)
        {
            try
            {
                EFWagons ef_wag = new EFWagons();
                // Получить список вагонов для переноса
                List<NumVag_Stpr1InStVag> list_car = ef_wag.GetNumVag_Stpr1InStVag(bis.doc_num, bis.napr == 2 ? true : false).ToList();

                return UpdateCountWagons(ref bis, list_car);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCountWagons(bis={0})", bis.GetFieldsAndValue()), servece_owner, eventID);
                return (int)errorTransfer.error_list_wagons;
            }
        }
        /// <summary>
        /// Обновить список вагонов
        /// </summary>
        /// <param name="bis"></param>
        /// <param name="list_car"></param>
        /// <returns></returns>
        public int UpdateCountWagons(ref RWBufferInputSostav bis, List<NumVag_Stpr1InStVag> list_car)
        {
            try
            {
                int result = 0;
                EFTKIS ef_tkis = new EFTKIS();
                if (list_car != null && list_car.Count() != bis.count_wagons)
                {
                    if (bis.count_wagons != null && bis.count_set_wagons != null)
                    {
                        // Вагоны были перенесены
                        // Удалим вагоны из системы RailCars
                        // TODO: Сделать код удаления вагонов из RailWay
                    }
                    // Определим новое количество вагонов

                    if (bis.count_wagons != null)
                    {
                        bis.list_update_wagons += String.Format(" Update(дата:{0}, было:{1}[{2}], стало:{3})", DateTime.Now, bis.count_wagons, bis.list_wagons, list_car != null ? (int?)list_car.Count() : null);
                        bis.status = (int)statusSting.Update;
                    }
                    bis.count_wagons = list_car.Count(); // Определим количество вагонов
                    bis.count_set_wagons = null;
                    bis.list_wagons = GetWagonsToString(list_car.Select(v => v.N_VAG).ToArray().ToList()); // Определим список вагонов
                    bis.list_no_set_wagons = null;
                    bis.close_comment = "Update";
                    bis.close = null;
                    result = ef_tkis.SaveRWBufferInputSostav(bis);
                }
                return result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCountWagons(bis={0}, list_car={0})", bis.GetFieldsAndValue(), list_car.GetFieldsAndValue()), servece_owner, eventID);
                return (int)errorTransfer.error_list_wagons;
            }
        }
        /// <summary>
        /// проверить изменения в количестве вагонов в составе
        /// </summary>
        /// <param name="bis"></param>
        /// <param name="doc"></param>
        protected void CheckChangeExistRWBufferInputSostav(RWBufferInputSostav bis, int doc, int? napr)
        {
            try
            {
                EFWagons ef_wag = new EFWagons();
                // Получим список вагонов
                List<NumVag_Stpr1InStVag> list_car = ef_wag.GetNumVag_Stpr1InStVag(doc, napr == 2 ? true : false).ToList();
                UpdateCountWagons(ref bis, list_car);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CheckChangeExistRWBufferInputSostav(bis={0}, doc={1}, napr={2})", bis.GetFieldsAndValue(), doc, napr), servece_owner, eventID);
            }
        }
        /// <summary>
        /// Найти и удалить из списка Oracle_InputSostav елемент doc
        /// </summary>
        /// <param name="list"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        protected bool DelExistRWBufferInputSostav(ref List<RWBufferInputSostav> list, int doc, int? napr)
        {
            bool Result = false;
            int index = list.Count() - 1;
            while (index >= 0)
            {
                if (list[index].doc_num == doc)
                {
                    CheckChangeExistRWBufferInputSostav(list[index], doc, napr); // количество вагонов
                    list.RemoveAt(index);
                    Result = true;
                }
                index--;
            }
            return Result;
        }
        /// <summary>
        /// Проверяет списки NumVagStpr1InStDoc и BufferInputSostav на повторяющие документы, оставляет в списке NumVagStpr1InStDoc - добавленные составы, BufferInputSostav - удаленные из КИС составы
        /// </summary>
        /// <param name="list_is"></param>
        /// <param name="list_ois"></param>
        protected void DelExistRWBufferInputSostav(ref List<NumVag_Stpr1InStDoc> list_is, ref List<RWBufferInputSostav> list_ois)
        {
            int index = list_is.Count() - 1;
            while (index >= 0)
            {
                if (DelExistRWBufferInputSostav(ref list_ois, list_is[index].ID_DOC, list_is[index].NAPR_IN_ST))
                {
                    list_is.RemoveAt(index);
                }
                index--;
            }
        }
        /// <summary>
        /// удалить строку состава отсутсвующего после переноса
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected int DeleteRWBufferInputSostav(List<RWBufferInputSostav> list)
        {
            EFRailWay ef_rw = new EFRailWay();
            EFTKIS ef_tkis = new EFTKIS();
            if (list == null || list.Count == 0) return 0;
            int delete = 0;
            int errors = 0;
            foreach (RWBufferInputSostav bis in list)
            {
                // Удалим вагоны из системы RailCars
                // TODO: Сделать код удаления вагонов из RailWay
                // Удалим вагоны из системы RailCars
                //transfer_rc.DeleteVagonsToDocInput(or_is.DocNum);
                bis.close = DateTime.Now;
                bis.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                bis.status = (int)statusSting.Delete;
                int res = ef_tkis.SaveRWBufferInputSostav(bis);
                if (res > 0) { 
                    delete++;
                    if (log_detali)
                    {
                        String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Таблица состояния переноса составов (по прибытию) по данным системы КИС - удален составов:(документ: {0}, дата: {1}, ID:{2}.", bis.doc_num, bis.datetime, bis.id).WriteWarning(servece_owner, this.eventID);
                    }
                }
                if (res < 1)
                {
                    String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Ошибка выполнения метода DeleteRWBufferInputSostav, удаление строки:{0} из таблицы состояния переноса составов (по прибытию) по данным системы КИС", bis.id).WriteError(servece_owner, this.eventID);
                    errors++;
                }
            }
            String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Таблица состояния переноса составов (по прибытию) по данным системы КИС, определенно удаленных в системе КИС {0} составов, удалено из таблицы {1}, ошибок удаления {2}.", list.Count(), delete, errors).WriteWarning(servece_owner, this.eventID);
            return delete;
        }
        /// <summary>
        /// Добавить новые составы появившиеся после переноса
        /// </summary>
        /// <param name="list"></param>
        protected int InsertRWBufferInputSostav(List<NumVag_Stpr1InStDoc> list)
        {
            if (list == null | list.Count == 0) return 0;
            int insers = 0;
            int errors = 0;
            foreach (NumVag_Stpr1InStDoc inp_s in list)
            {
                int res = SaveRWBufferInputSostav(inp_s, statusSting.Insert);
                if (res > 0) insers++;
                if (res < 1)
                {
                    String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Ошибка выполнения метода InsertRWBufferInputSostav, добавления строки состава по данным системы КИС(№ документа:{0}, дата:{1}) в таблицу состояния переноса составов по прибытию RWBufferInputSostav", inp_s.ID_DOC, inp_s.DATE_IN_ST).WriteError(servece_owner, this.eventID);
                    errors++;
                }
            }
            String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Таблица состояния переноса составов (по прибытию) по данным системы КИС, определенно добавленных в системе КИС {0} составов, добавлено в таблицу {1}, ошибок добавления {2}.", list.Count(), insers, errors).WriteWarning(servece_owner, this.eventID);
            return insers;
        }
        #endregion

        #region Таблица переноса составов из КИС [RWBufferOutputSostav]
        /// <summary>
        /// Сохранить состав из КИС
        /// </summary>
        /// <param name="out_sostav"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        protected int SaveRWBufferOutputSostav(NumVag_Stpr1OutStDoc out_sostav, statusSting status)
        {

            try
            {
                EFTKIS ef_tkis = new EFTKIS();                
                EFWagons ef_wag = new EFWagons();
                // Получим список вагонов
                List<NumVag_Stpr1OutStVag> list_car = ef_wag.GetNumVag_Stpr1OutStVag(out_sostav.ID_DOC, out_sostav.NAPR_OUT_ST == 2 ? true : false).ToList();
               
                return ef_tkis.SaveRWBufferOutputSostav(new RWBufferOutputSostav()
                {
                    id = 0,
                    datetime = out_sostav.DATE_OUT_ST,
                    doc_num = out_sostav.ID_DOC,
                    id_station_on_kis = out_sostav.ST_OUT_ST != null ? (int)out_sostav.ST_OUT_ST : 0,
                    way_num_kis = out_sostav.N_PUT_OUT_ST != null ? (int)out_sostav.N_PUT_OUT_ST : 0,
                    napr = out_sostav.NAPR_OUT_ST != null ? (int)out_sostav.NAPR_OUT_ST : 0,
                    id_station_from_kis = out_sostav.K_STAN != null ? (int)out_sostav.K_STAN : 0,
                    count_wagons = list_car!=null ? (int?)list_car.Count() : null, // Определим количество вагонов
                    count_set_wagons = null,
                    close = null,
                    close_user = null,
                    close_comment = null,
                    status = (int)status,
                    list_wagons = list_car!=null ? GetWagonsToString(list_car.Select(v => v.N_VAG).ToArray().ToList()) : null, // Определим список вагонов
                    list_no_set_wagons = null,
                    message = null, 
                });
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveRWBufferOutputSostav(inp_sostav={0}, status={1})", out_sostav.GetFieldsAndValue(), status), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Обновить список вагонов
        /// </summary>
        /// <param name="bos"></param>
        /// <returns></returns>
        public int UpdateCountWagons(ref RWBufferOutputSostav bos)
        {
            try
            {
                EFWagons ef_wag = new EFWagons();
                // Получить список вагонов для переноса
                List<NumVag_Stpr1OutStVag> list_car = ef_wag.GetNumVag_Stpr1OutStVag(bos.doc_num, bos.napr == 2 ? true : false).ToList();

                return UpdateCountWagons(ref bos, list_car);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCountWagons(bos={0})", bos.GetFieldsAndValue()), servece_owner, eventID);
                return (int)errorTransfer.error_list_wagons;
            }
        }
        /// <summary>
        /// Обновить список вагонов
        /// </summary>
        /// <param name="bos"></param>
        /// <param name="list_car"></param>
        /// <returns></returns>
        public int UpdateCountWagons(ref RWBufferOutputSostav bos, List<NumVag_Stpr1OutStVag> list_car)
        {
            try
            {
                int result = 0;
                EFTKIS ef_tkis = new EFTKIS();
                if (list_car != null && list_car.Count() != bos.count_wagons)
                {
                    if (bos.count_wagons != null && bos.count_set_wagons != null)
                    {
                        // Вагоны были перенесены
                        // Удалим вагоны из системы RailCars
                        // TODO: Сделать код удаления вагонов из RailWay
                    }
                    // Определим новое количество вагонов

                    if (bos.count_wagons != null)
                    {
                        //string list_update_wagons = bos.list_update_wagons += String.Format(" Update(дата:{0}, было:{1}[{2}], стало:{3})", DateTime.Now, bos.count_wagons, bos.list_wagons, list_car != null ? (int?)list_car.Count() : null);
                        //// Обрезка строки если превышает 4000
                        //if (list_update_wagons.Length > 4000) {
                        //    list_update_wagons = list_update_wagons.Substring(0, list_update_wagons.Length - 4000);
                        //}
                        bos.list_update_wagons += String.Format(" Update(дата:{0}, было:{1}[{2}], стало:{3})", DateTime.Now, bos.count_wagons, bos.list_wagons, list_car != null ? (int?)list_car.Count() : null);
                        bos.status = (int)statusSting.Update;                    
                    }
                    bos.count_wagons = list_car.Count(); // Определим количество вагонов
                    bos.count_set_wagons = null;
                    bos.list_wagons = GetWagonsToString(list_car.Select(v => v.N_VAG).ToArray().ToList()); // Определим список вагонов
                    bos.list_no_set_wagons = null;
                    bos.close_comment = "Update";                    
                    bos.close = null;
                    result = ef_tkis.SaveRWBufferOutputSostav(bos);
                }
                return result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCountWagons(bos={0}, list_car={0})", bos.GetFieldsAndValue(), list_car.GetFieldsAndValue()), servece_owner, eventID);
                return (int)errorTransfer.error_list_wagons;
            }
        }
        /// <summary>
        /// проверить изменения в количестве вагонов в составе
        /// </summary>
        /// <param name="bos"></param>
        /// <param name="doc"></param>
        protected void CheckChangeExistRWBufferOutputSostav(RWBufferOutputSostav bos, int doc, int? napr)
        {
            try
            {
                EFWagons ef_wag = new EFWagons();
                // Получим список вагонов
                List<NumVag_Stpr1OutStVag> list_car = ef_wag.GetNumVag_Stpr1OutStVag(doc, napr == 2 ? true : false).ToList();
                UpdateCountWagons(ref bos, list_car);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CheckChangeExistRWBufferOutputSostav(bos={0}, doc={1}, napr={2})", bos.GetFieldsAndValue(), doc, napr), servece_owner, eventID);
            }
        }
        /// <summary>
        /// Найти и удалить из списка RWBufferOutputSostav елемент doc
        /// </summary>
        /// <param name="list"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        protected bool DelExistRWBufferOutputSostav(ref List<RWBufferOutputSostav> list, int doc, int? napr)
        {
            bool Result = false;
            int index = list.Count() - 1;
            while (index >= 0)
            {
                if (list[index].doc_num == doc)
                {
                    CheckChangeExistRWBufferOutputSostav(list[index], doc, napr); // количество вагонов
                    list.RemoveAt(index);
                    Result = true;
                }
                index--;
            }
            return Result;
        }
        /// <summary>
        /// Проверяет списки NumVagStpr1InStDoc и RWBufferOutputSostav на повторяющие документы, оставляет в списке NumVagStpr1InStDoc - добавленные составы, RWBufferOutputSostav - удаленные из КИС составы
        /// </summary>
        /// <param name="list_is"></param>
        /// <param name="list_ois"></param>
        protected void DelExistRWBufferOutputSostav(ref List<NumVag_Stpr1OutStDoc> list_is, ref List<RWBufferOutputSostav> list_ois)
        {
            int index = list_is.Count() - 1;
            while (index >= 0)
            {
                if (DelExistRWBufferOutputSostav(ref list_ois, list_is[index].ID_DOC, list_is[index].NAPR_OUT_ST))
                {
                    list_is.RemoveAt(index);
                }
                index--;
            }
        }
        /// <summary>
        /// удалить строку состава отсутсвующего после переноса
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected int DeleteRWBufferOutputSostav(List<RWBufferOutputSostav> list)
        {
            try
            {
                EFRailWay ef_rw = new EFRailWay();
                EFTKIS ef_tkis = new EFTKIS();
                if (list == null || list.Count == 0) return 0;
                int delete = 0;
                int errors = 0;
                foreach (RWBufferOutputSostav bis in list)
                {
                    // Удалим вагоны из системы RailCars
                    // TODO: Сделать код удаления вагонов из RailWay
                    // Удалим вагоны из системы RailCars
                    //transfer_rc.DeleteVagonsToDocInput(or_is.DocNum);
                    bis.close = DateTime.Now;
                    bis.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                    bis.status = (int)statusSting.Delete;
                    int res = ef_tkis.SaveRWBufferOutputSostav(bis);
                    if (res > 0)
                    {
                        delete++;
                        if (log_detali)
                        {
                            String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Таблица состояния переноса составов (по отправке) по данным системы КИС - удален составов:(документ: {0}, дата: {1}, ID:{2}.", bis.doc_num, bis.datetime, bis.id).WriteWarning(servece_owner, this.eventID);
                        }
                    }
                    if (res < 1)
                    {
                        String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Ошибка выполнения метода DeleteRWBufferOutputSostav, удаление строки:{0} из таблицы состояния переноса составов (по отправке) по данным системы КИС", bis.id).WriteError(servece_owner, this.eventID);
                        errors++;
                    }
                }
                String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Таблица состояния переноса составов (по отправке) по данным системы КИС, определенно удаленных в системе КИС {0} составов, удалено из таблицы {1}, ошибок удаления {2}.", list.Count(), delete, errors).WriteWarning(servece_owner, this.eventID);
                return delete;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteRWBufferOutputSostav(list={0})", list.GetFieldsAndValue()), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Добавить новые составы появившиеся после переноса
        /// </summary>
        /// <param name="list"></param>
        protected int InsertRWBufferOutputSostav(List<NumVag_Stpr1OutStDoc> list)
        {
            try
            {
                if (list == null | list.Count == 0) return 0;
                int insers = 0;
                int errors = 0;
                foreach (NumVag_Stpr1OutStDoc inp_s in list)
                {
                    int res = SaveRWBufferOutputSostav(inp_s, statusSting.Insert);
                    if (res > 0) insers++;
                    if (res < 1)
                    {
                        String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Ошибка выполнения метода InsertRWBufferOutputSostav, добавления строки состава по данным системы КИС(№ документа:{0}, дата:{1}) в таблицу состояния переноса составов по отправке RWBufferOutputSostav", inp_s.ID_DOC, inp_s.DATE_OUT_ST).WriteError(servece_owner, this.eventID);
                        errors++;
                    }
                }
                String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Таблица состояния переноса составов (по отправке) по данным системы КИС, определенно добавленных в системе КИС {0} составов, добавлено в таблицу {1}, ошибок добавления {2}.", list.Count(), insers, errors).WriteWarning(servece_owner, this.eventID);
                return insers;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("InsertRWBufferOutputSostav(list={0})", list.GetFieldsAndValue()), servece_owner, eventID);
                return -1;
            }
        }
        #endregion


        #region дополнительные методы к RWOperation
        /// <summary>
        /// Поставить вагон в сисему RailWay по данным PromVagon
        /// </summary>
        /// <param name="prom_vagon"></param>
        /// <param name="set_operation"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public Cars SetCarsToRailWay(Prom_Vagon prom_vagon, RW.RWOperation.SetCarOperation set_operation, IOperation operation)
        {
            try
            {
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                CarsInpDelivery delivery = CreateCarsInpDelivery(prom_vagon);
                return rw_oper.SetCarsToRailWay(prom_vagon.N_VAG, prom_vagon.N_NATUR * -1, delivery, set_operation, operation);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarsToRailWay(prom_vagon={0}, set_operation={1}, operation={2})", prom_vagon.GetFieldsAndValue(), set_operation, operation), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Поставить вагон в сисему RailWay по данным PromNatHist
        /// </summary>
        /// <param name="prom_nathist"></param>
        /// <param name="set_operation"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public Cars SetCarsToRailWay(Prom_NatHist prom_nathist, RW.RWOperation.SetCarOperation set_operation, IOperation operation)
        {
            try
            {
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                CarsInpDelivery delivery = CreateCarsInpDelivery(prom_nathist);
                return rw_oper.SetCarsToRailWay(prom_nathist.N_VAG, prom_nathist.N_NATUR * -1, delivery, set_operation, operation);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarsToRailWay(prom_nathist={0}, set_operation={1}, operation={2})", prom_nathist.GetFieldsAndValue(), set_operation, operation), servece_owner, eventID);
                return null;
            }
        }

        /// <summary>
        /// Создать справочник SAP Входящие поставки по данным PromVagon
        /// </summary>
        /// <param name="prom_vagon"></param>
        /// <returns></returns>
        public CarsInpDelivery CreateCarsInpDelivery(Prom_Vagon prom_vagon)
        {
            try
            {
                RWReference rw_ref = new RWReference(base.servece_owner, reference_kis);
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                DateTime dt_oper = DateTime.Parse(prom_vagon.D_PR_DD.ToString() + "-" + prom_vagon.D_PR_MM.ToString() + "-" + prom_vagon.D_PR_YY.ToString() + " " + prom_vagon.T_PR_HH.ToString() + ":" + prom_vagon.T_PR_MI.ToString() + ":00", CultureInfo.CreateSpecificCulture("ru-RU"));
                // Определим код груза
                return rw_oper.CreateCarsInpDelivery(prom_vagon.N_VAG,
                    (prom_vagon.N_NATUR * -1),
                    dt_oper,
                    "-",
                    prom_vagon.NPP !=null ? (int)prom_vagon.NPP : 0,
                    prom_vagon != null && prom_vagon.KOD_STRAN != null ? (int)prom_vagon.KOD_STRAN : 0,
                    rw_ref.GetCorrectCodeETSNGOfKis(prom_vagon.K_GR),
                    prom_vagon != null && prom_vagon.WES_GR != null ? (float)prom_vagon.WES_GR : 0,
                    7932);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCarsInpDelivery(prom_vagon={0})", prom_vagon.GetFieldsAndValue()), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Создать справочник SAP Входящие поставки по данным PromNatHist
        /// </summary>
        /// <param name="prom_nathist"></param>
        /// <returns></returns>
        public CarsInpDelivery CreateCarsInpDelivery(Prom_NatHist prom_nathist)
        {
            try
            {
                RWReference rw_ref = new RWReference(base.servece_owner, reference_kis);
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                DateTime dt_oper = DateTime.Parse(prom_nathist.D_PR_DD.ToString() + "-" + prom_nathist.D_PR_MM.ToString() + "-" + prom_nathist.D_PR_YY.ToString() + " " + prom_nathist.T_PR_HH.ToString() + ":" + prom_nathist.T_PR_MI.ToString() + ":00", CultureInfo.CreateSpecificCulture("ru-RU"));
                // Определим код груза
                return rw_oper.CreateCarsInpDelivery(prom_nathist.N_VAG,
                    (prom_nathist.N_NATUR * -1),
                    dt_oper,
                    "-",
                    (int)prom_nathist.NPP,
                    prom_nathist != null && prom_nathist.KOD_STRAN != null ? (int)prom_nathist.KOD_STRAN : 0,
                    rw_ref.GetCorrectCodeETSNGOfKis(prom_nathist.K_GR),
                    prom_nathist != null && prom_nathist.WES_GR != null ? (float)prom_nathist.WES_GR : 0,
                    7932);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCarsInpDelivery(prom_nathist={0})", prom_nathist.GetFieldsAndValue()), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Создать справочник SAP Исходящие поставки по данным PromNatHist
        /// </summary>
        /// <param name="num"></param>
        /// <param name="natur"></param>
        /// <param name="dt_out_amkr"></param>
        /// <returns></returns>
        public CarsOutDelivery CreateCarsOutDelivery(int num, int natur, DateTime dt_out_amkr)
        {
            try
            {
                EFWagons ef_wag = new EFWagons();
                //PromNatHist pnh = ef_wag.GetNatHistSD(natur, num, dt_out_amkr.Day, dt_out_amkr.Month, dt_out_amkr.Year, dt_out_amkr.Hour, dt_out_amkr.Minute);
                Prom_NatHist pnh = ef_wag.GetSendingProm_NatHistOfNaturNumDateTime(natur, num, dt_out_amkr.Day, dt_out_amkr.Month, dt_out_amkr.Year, dt_out_amkr.Hour, dt_out_amkr.Minute);
                // Определим исходящие поставки
                return CreateCarsOutDelivery(pnh);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCarsOutDelivery(num={0}, natur={1}, dt_out_amkr={2})", num, natur, dt_out_amkr), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Создать справочник SAP Исходящие поставки 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="dt_out_amkr"></param>
        /// <returns></returns>
        public CarsOutDelivery CreateCarsOutDelivery(int num, DateTime dt_out_amkr)
        {
            try
            {
                EFWagons ef_wag = new EFWagons();
                //PromNatHist pnh = ef_wag.GetNatHistSD(num, dt_out_amkr.Day, dt_out_amkr.Month, dt_out_amkr.Year, dt_out_amkr.Hour, dt_out_amkr.Minute);
                Prom_NatHist pnh = ef_wag.GetSendingProm_NatHistOfNumDateTime(num, dt_out_amkr.Day, dt_out_amkr.Month, dt_out_amkr.Year, dt_out_amkr.Hour, dt_out_amkr.Minute);
                // Определим исходящие поставки
                return CreateCarsOutDelivery(pnh);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCarsOutDelivery(num={0}, dt_out_amkr={1})", dt_out_amkr), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Создать справочник SAP Исходящие поставки по данным PromNatHist
        /// </summary>
        /// <param name="pnh_out"></param>
        /// <returns></returns>
        public CarsOutDelivery CreateCarsOutDelivery(Prom_NatHist pnh_out)
        {
            try
            {
                RWReference rw_ref = new RWReference(base.servece_owner, reference_kis);
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                EFWagons ef_wag = new EFWagons();

                DateTime dt_out_amkr = DateTime.Parse(pnh_out.D_SD_DD.ToString() + "-" + pnh_out.D_SD_MM.ToString() + "-" + pnh_out.D_SD_YY.ToString() + " " + pnh_out.T_SD_HH.ToString() + ":" + pnh_out.T_SD_MI.ToString() + ":00", CultureInfo.CreateSpecificCulture("ru-RU"));

                List<NumVagStpr1OutStDoc> list_out_sostav = ef_wag.GetSTPR1OutStDoc(dt_out_amkr.AddDays(-3), dt_out_amkr).ToList();
                List<NumVagStpr1OutStVag> list_out_car = ef_wag.GetSTPR1OutStDocOfCarAndDoc(new int[] { pnh_out.N_VAG }, list_out_sostav.Select(s => s.ID_DOC).ToArray()).ToList();

                int? typik = null;
                NumVagStran stan_kis;
                int? station;
                string rem;
                if (list_out_car != null && list_out_car.Count() > 0)
                {
                    NumVagStpr1OutStVag car_out = list_out_car.OrderByDescending(v => v.ID_DOC).FirstOrDefault();
                    stan_kis = car_out.STRAN_OUT_ST != null ? ef_wag.GetNumVagStranOfCodeEurope((int)car_out.STRAN_OUT_ST) : null;
                    typik = car_out.N_TUP_OUT_ST != null ? rw_ref.GetIDDeadlockOfKis((int)car_out.N_TUP_OUT_ST, true) : null;
                    station = car_out.ST_NAZN_OUT_ST;
                    rem = car_out.REM_IN_ST;
                }
                else
                {
                    stan_kis = ef_wag.GetNumVagStranOfCodeSNG(pnh_out.KOD_STRAN);
                    Stations stations = rw_ref.GetStations(pnh_out.K_ST_NAZN != null ? (int)pnh_out.K_ST_NAZN : 0, true);
                    station = stations != null ? stations.code_uz : null;
                    rem = "";
                }
                // Определим код груза
                return rw_oper.CreateCarsOutDelivery(pnh_out.N_VAG,
                    typik,
                    (stan_kis != null ? (int?)stan_kis.KOD_STRAN : null),
                    station,
                    rem,
                    rw_ref.GetCorrectCodeETSNGOfKis(pnh_out.K_GR_T),
                    pnh_out.WES_GR_T != null ? (float)pnh_out.WES_GR_T : 0);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCarsOutDelivery(pnh_out={0})", pnh_out.GetFieldsAndValue()), servece_owner, eventID);
                return null;
            }
        }
        #endregion

        #region дополнительные методы к EFWagons
        /// <summary>
        /// Коррекция Промышленая\Промышленная Керамет
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num_vag"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_station_kis"></param>
        /// <returns></returns>
        public Prom_NatHist GetCorrectNatHist(int natur, int num_vag, DateTime dt_amkr, int id_station_kis)
        {
            EFWagons ef_wag = new EFWagons();
            Prom_NatHist pnh = ef_wag.GetArrivalProm_NatHistOfNaturNumStationDate(natur, num_vag, id_station_kis, dt_amkr.Day, dt_amkr.Month, dt_amkr.Year);
            if (pnh == null & id_station_kis == 18)
            {
                // Если промышленная, попробовать Промышленная-керамет
                pnh = ef_wag.GetArrivalProm_NatHistOfNaturNumStationDate(natur, num_vag, 81, dt_amkr.Day, dt_amkr.Month, dt_amkr.Year);
            }
            return pnh;
        }
        /// <summary>
        /// Коррекция Промышленая\Промышленная Керамет
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num_vag"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_station_kis"></param>
        /// <returns></returns>
        public Prom_Vagon GetCorrectVagon(int natur, int num_vag, DateTime dt_amkr, int id_station_kis)
        {
            EFWagons ef_wag = new EFWagons();
            Prom_Vagon pv = ef_wag.GetArrivalProm_VagonOfNaturNumStationDate(natur, num_vag, id_station_kis, dt_amkr.Day, dt_amkr.Month, dt_amkr.Year);
            if (pv == null & id_station_kis == 18)
            {
                // Если промышленная, попробовать Промышленная-керамет
                pv = ef_wag.GetArrivalProm_VagonOfNaturNumStationDate(natur, num_vag, 81, dt_amkr.Day, dt_amkr.Month, dt_amkr.Year);
            }
            return pv;
        }
        /// <summary>
        /// Вагон есть в списке NatHist по указаной натурке по указаному времени
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num_vag"></param>
        /// <param name="dt_out"></param>
        /// <returns></returns>
        public bool IsVagonOfSendingNatHistNatur(int natur, int num_vag, DateTime dt_out) {

            EFWagons ef_wag = new EFWagons();
            Prom_NatHist pnh = ef_wag.GetSendingProm_NatHistOfNaturNumDateTime(natur, num_vag, dt_out.Day, dt_out.Month, dt_out.Year, dt_out.Hour, dt_out.Minute);
            return pnh!= null ? true : false;
        }
        #endregion

        #region ПЕРЕНОС И ОБНАВЛЕНИЕ ВАГОНОВ ИЗ СИСТЕМЫ КИС в СИСТЕМУ RailWAY

        /// <summary>
        /// Обновить таблицу буфер прибывающих составов из станций УЗ по системы КИС
        /// </summary>
        /// <returns></returns>
        public int CopyBufferArrivalSostavOfKIS()
        {
            int res_rw = CopyBufferArrivalSostavOfKIS(this.day_control_arrival_kis_add_data);
            return res_rw;
        }
        /// <summary>
        /// Обновить таблицу буфер отправленных составов на станции УЗ по системы КИС
        /// </summary>
        /// <returns></returns>
        public int CopyBufferSendingSostavOfKIS()
        {
            int res_rw = CopyBufferSendingSostavOfKIS(this.day_control_sending_kis_add_data);
            return res_rw;
        }
        /// <summary>
        /// Обновить таблицу буфер прибывающих составов из станций УЗ по системы КИС
        /// </summary>
        /// <param name="day_control_add_natur"></param>
        /// <returns></returns>
        public int CopyBufferArrivalSostavOfKIS(int day_control_add_natur)
        {
            EFTKIS ef_tkis = new EFTKIS();
            EFWagons ef_wag = new EFWagons();

            int errors = 0;
            int normals = 0;
            // список новых составов в системе КИС
            List<Prom_Sostav> list_newsostav = new List<Prom_Sostav>();
            // список уже перенесенных в RailWay составов в системе КИС (с учетом периода контроля dayControllingAddNatur)
            List<Prom_Sostav> list_oldsostav = new List<Prom_Sostav>();
            // список уже перенесенных в RailWay составов (с учетом периода контроля dayControllingAddNatur)
            List<RWBufferArrivalSostav> list_arrivalsostav = new List<RWBufferArrivalSostav>();
            try
            {
                // Считаем дату последненго состава
                DateTime? lastDT = ef_tkis.GetLastDateTimeRWBufferArrivalSostav();
                if (lastDT != null)
                {
                    // Данные есть получим новые
                    list_newsostav = ef_wag.GetInputProm_Sostav(((DateTime)lastDT).AddSeconds(1), DateTime.Now, false).ToList();
                    list_oldsostav = ef_wag.GetInputProm_Sostav(((DateTime)lastDT).AddDays(day_control_add_natur * -1), ((DateTime)lastDT).AddSeconds(1), false).ToList();
                    list_arrivalsostav = ef_tkis.GetRWBufferArrivalSostav(((DateTime)lastDT).AddDays(day_control_add_natur * -1), ((DateTime)lastDT).AddSeconds(1)).ToList();
                }
                else
                {
                    // Таблица пуста получим первый раз
                    list_newsostav = ef_wag.GetInputProm_Sostav(DateTime.Now.AddDays(day_control_add_natur * -1), DateTime.Now, false).ToList();
                }
                // Переносим информацию по новым составам
                if (list_newsostav.Count() > 0)
                {
                    foreach (Prom_Sostav ps in list_newsostav)
                    {

                        int res = SaveRWBufferArrivalSostav(ps, statusSting.Normal);
                        if (res > 0) normals++;
                        if (res < 0) { errors++; }
                    }
                    string mess_new = String.Format("[RailWay] Таблица состояния переноса составов зашедших на АМКР по данным системы КИС (определено новых составов:{0}, перенесено:{1}, ошибок переноса:{2}).", list_newsostav.Count(), normals, errors);
                    mess_new.WriteInformation(servece_owner, this.eventID);
                    if (list_newsostav.Count() > 0) mess_new.WriteEvents(errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID);
                }
                // Обновим информацию по составам которые были перенесены
                if (list_oldsostav.Count() > 0 & list_arrivalsostav.Count() > 0)
                {
                    List<Prom_Sostav> list_ps = new List<Prom_Sostav>();
                    list_ps = list_oldsostav;
                    List<RWBufferArrivalSostav> list_as = new List<RWBufferArrivalSostav>();
                    list_as = list_arrivalsostav.Where(a => a.status != (int)statusSting.Delete).ToList();
                    DelExistRWBufferArrivalSostav(ref list_ps, ref list_as);
                    int ins = InsertRWBufferArrivalSostav(list_ps);
                    int del = DeleteRWBufferArrivalSostav(list_as);
                    string mess_upd = String.Format("[RailWay] Таблица состояния переноса составов зашедших на АМКР по данным системы КИС (определено добавленных составов:{0}, перенесено:{1}, определено удаленных составов:{2}, удалено:{3}).",
                    list_ps.Count(), ins, list_as.Count(), del);
                    mess_upd.WriteInformation(servece_owner, this.eventID);
                    if (list_ps.Count() > 0 | list_as.Count() > 0) mess_upd.WriteEvents(EventStatus.Ok, servece_owner, eventID);
                    normals += ins;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CopyBufferArrivalSostavOfKIS(day_control_add_natur={0})", day_control_add_natur), servece_owner, eventID);
                return -1;
            }
            return normals;
        }
        /// <summary>
        /// Обновить таблицу буфер отправленных составов на УЗ по системы КИС
        /// </summary>
        /// <param name="day_control_add_natur"></param>
        /// <returns></returns>
        public int CopyBufferSendingSostavOfKIS(int day_control_add_natur)
        {
            EFTKIS ef_tkis = new EFTKIS();
            EFWagons ef_wag = new EFWagons();

            int errors = 0;
            int normals = 0;
            // список новых составов в системе КИС
            List<Prom_Sostav> list_newsostav = new List<Prom_Sostav>();
            // список уже перенесенных в RailWay составов в системе КИС (с учетом периода контроля dayControllingAddNatur)
            List<Prom_Sostav> list_oldsostav = new List<Prom_Sostav>();
            // список уже перенесенных в RailWay составов (с учетом периода контроля dayControllingAddNatur)
            List<RWBufferSendingSostav> list_sendingsostav = new List<RWBufferSendingSostav>();
            try
            {
                // Считаем дату последненго состава
                DateTime? lastDT = ef_tkis.GetLastDateTimeRWBufferSendingSostav();
                if (lastDT != null)
                {
                    // Данные есть получим новые
                    list_newsostav = ef_wag.GetOutputProm_Sostav(((DateTime)lastDT).AddSeconds(1), DateTime.Now, false).ToList();
                    list_oldsostav = ef_wag.GetOutputProm_Sostav(((DateTime)lastDT).AddDays(day_control_add_natur * -1), ((DateTime)lastDT).AddSeconds(1), false).ToList();
                    list_sendingsostav = ef_tkis.GetRWBufferSendingSostav(((DateTime)lastDT).AddDays(day_control_add_natur * -1), ((DateTime)lastDT).AddSeconds(1)).ToList();
                }
                else
                {
                    // Таблица пуста получим первый раз
                    list_newsostav = ef_wag.GetOutputProm_Sostav(DateTime.Now.AddDays(day_control_add_natur * -1), DateTime.Now, false).ToList();
                }
                // Переносим информацию по новым составам
                if (list_newsostav.Count() > 0)
                {
                    foreach (Prom_Sostav ps in list_newsostav)
                    {

                        int res = SaveRWBufferSendingSostav(ps, statusSting.Normal);
                        if (res > 0) normals++;
                        if (res < 0) { errors++; }
                    }
                    string mess_new = String.Format("[RailWay] Таблица состояния переноса составов сданных на УЗ по данным системы КИС (определено новых составов:{0}, перенесено:{1}, ошибок переноса:{2}).", list_newsostav.Count(), normals, errors);
                    mess_new.WriteInformation(servece_owner, this.eventID);
                    if (list_newsostav.Count() > 0) mess_new.WriteEvents(errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID);
                }
                // Обновим информацию по составам которые были перенесены
                if (list_oldsostav.Count() > 0 & list_sendingsostav.Count() > 0)
                {
                    List<Prom_Sostav> list_ps = new List<Prom_Sostav>();
                    list_ps = list_oldsostav;
                    List<RWBufferSendingSostav> list_ss = new List<RWBufferSendingSostav>();
                    list_ss = list_sendingsostav.Where(a => a.status != (int)statusSting.Delete).ToList();
                    DelExistRWBufferSendingSostav(ref list_ps, ref list_ss);
                    int ins = InsertRWBufferSendingSostav(list_ps);
                    int del = DeleteRWBufferSendingSostav(list_ss);
                    string mess_upd = String.Format("[RailWay] Таблица состояния переноса составов сданных на УЗ по данным системы КИС (определено добавленных составов:{0}, перенесено:{1}, определено удаленных составов:{2}, удалено:{3}).",
                    list_ps.Count(), ins, list_ss.Count(), del);
                    mess_upd.WriteInformation(servece_owner, this.eventID);
                    if (list_ps.Count() > 0 | list_ss.Count() > 0) mess_upd.WriteEvents(EventStatus.Ok, servece_owner, eventID);
                    normals += ins;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CopyBufferSendingSostavOfKIS(day_control_add_natur={0})", day_control_add_natur), servece_owner, eventID);
                return -1;
            }
            return normals;
        }
        /// <summary>
        /// Перенести список составов на Ж.Д АМКР из УЗ (по данным КИС)
        /// </summary>
        /// <returns></returns>
        public int TransferArrivalKISToRailWay()
        {
            try
            {
                EFTKIS ef_tkis = new EFTKIS();
                int close = 0;
                IQueryable<RWBufferArrivalSostav> list_noClose = ef_tkis.GetRWBufferArrivalSostavNoClose();
                if (list_noClose == null || list_noClose.Count() == 0) return 0;
                foreach (RWBufferArrivalSostav bas in list_noClose.ToList())
                {
                    try
                    {
                        string mess_put = String.Format("[RailWay] Состав (натурный лист: {0}, дата: {1}, ID строки буфера переноса: {2}), прибывающий с УЗ на станцию АМКР (id_kis:{3}) по данным системы КИС", bas.natur, bas.datetime, bas.id, bas.id_station_kis);
                        RWBufferArrivalSostav bas_result = new RWBufferArrivalSostav();
                        bas_result = bas;
                        // 1. Поставим состав на путь станции АМКР системы RailWay
                        int res_put = SetWayRailWayOfKIS(ref bas_result);
                        // 2. Обновление составов на пути станции АМКР системы RailWay
                        int res_upd = UpdWayRailWayOfKIS(ref bas_result);

                        //Закрыть состав
                        if (bas_result.count_wagons != null & bas_result.count_nathist != null & bas_result.count_set_wagons != null & bas_result.count_set_nathist != null
                            & bas_result.count_wagons == bas_result.count_nathist & bas_result.count_wagons == bas_result.count_set_wagons & bas_result.count_wagons == bas_result.count_set_nathist)
                        {
                            bas_result.close = DateTime.Now;
                            bas_result.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                            int res_close = ef_tkis.SaveRWBufferArrivalSostav(bas_result);
                            mess_put += " - перенесен и закрыт";
                            mess_put.WriteEvents(res_close > 0 ? EventStatus.Ok : EventStatus.Error, servece_owner, eventID);
                            close++;
                        }
                    }
                    catch (Exception e)
                    {
                        e.WriteError(String.Format("[RailWay] Ошибка обработки строки буфера переноса состава (натурный лист: {0}, дата: {1}, ID строки буфера переноса: {2})", bas.natur, bas.datetime, bas.id), servece_owner, eventID);
                    }
                }
                return close;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferArrivalKISToRailWay()"), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Перенести список составов на Ж.Д УЗ из АМКР (по данным КИС)
        /// </summary>
        /// <returns></returns>
        public int TransferSendingKISToRailWay()
        {
            try
            {
                EFTKIS ef_tkis = new EFTKIS();
                int close = 0;
                List<RWBufferSendingSostav> list_noClose = ef_tkis.GetRWBufferSendingSostavNoClose().ToList();

                if (list_noClose == null || list_noClose.Count() == 0) return 0;
                foreach (RWBufferSendingSostav bas in list_noClose)
                {
                    try
                    {
                        string mess_put = String.Format("[RailWay] Состав (натурный лист: {0}, дата: {1}, ID строки буфера переноса: {2}), отправленный на станцию УЗ (id_kis:{3}) по данным системы КИС", bas.natur, bas.datetime, bas.id, bas.id_station_on_kis);
                        RWBufferSendingSostav bss_result = new RWBufferSendingSostav();
                        bss_result = bas;
                        // Поставим состав на путь станции УЗ системы RailWay
                        int res_put = SetWayRailWayOfKIS(ref bss_result);

                        //Закрыть состав
                        if (bss_result.count_nathist != null & bss_result.count_set_nathist != null
                            & bss_result.count_nathist == bss_result.count_set_nathist)
                        {
                            bss_result.close = DateTime.Now;
                            bss_result.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                            int res_close = ef_tkis.SaveRWBufferSendingSostav(bss_result);
                            mess_put += " - перенесен и закрыт";
                            mess_put.WriteEvents(res_close > 0 ? EventStatus.Ok : EventStatus.Error, servece_owner, eventID);
                            close++;
                        }
                    }
                    catch (Exception e)
                    {
                        e.WriteError(String.Format("[RailWay] Ошибка обработки строки буфера переноса состава (натурный лист: {0}, дата: {1}, ID строки буфера переноса: {2})", bas.natur, bas.datetime, bas.id), servece_owner, eventID);
                    }
                }
                return close;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferSendingKISToRailWay()"), servece_owner, eventID);
                return -1;
            }
        }

        #region ПОСТАВИМ НА ПУТЬ АМКР СИСТЕМЫ RAILWAY ПО ДАННЫМ PROM_VAG
        /// <summary>
        /// Принять вагоны состава на путь станции по данным КИС 
        /// </summary>
        /// <param name="bas_sostav"></param>
        /// <param name="first"></param>
        /// <param name="secondary"></param>
        /// <returns></returns>
        public int SetWayRailWayOfKIS(ref RWBufferArrivalSostav bas_sostav)
        {
            try
            {
                string mess_transf = String.Format("cостава (натурный лист: {0}, дата: {1}, ID строки буфера переноса: {2}), прибывающий с УЗ на станцию АМКР",
                    bas_sostav.natur, bas_sostav.datetime, bas_sostav.id);
                string mess_transf1 = " по данным системы КИС.";
                string mess_arr_sostav = "[RailWay] Перенос " + mess_transf;
                string mess_error_arr_sostav = "[RailWay] Ошибка переноса " + mess_transf;

                RWReference rw_ref = new RWReference(base.servece_owner, reference_kis); // создавать содержимое справочника из данных КИС
                EFWagons ef_wag = new EFWagons();
                EFTKIS ef_tkis = new EFTKIS();

                int id_stations_rw = 0;
                int id_ways_rw = 0;

                id_stations_rw = rw_ref.GetIDStationsOfKIS(bas_sostav.id_station_kis);
                if (id_stations_rw <= 0)
                {
                    String.Format(mess_error_arr_sostav + mess_transf1 + " - ID станции: {0} не определён в справочнике системы RailWay", bas_sostav.id_station_kis).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_stations;
                }
                if (id_stations_rw == 26) id_stations_rw = 27; // Коррекция Промышленная Керамет -> 'это промышленная
                // Определим путь на станции система RailCars
                id_ways_rw = rw_ref.GetIDDefaultWayOfStation(id_stations_rw, bas_sostav.way_num.ToString());
                if (id_ways_rw <= 0)
                {
                    String.Format(mess_error_arr_sostav + mess_transf1 + " - ID пути: {0} станции: {1} не определён в справочнике системы RailWay", bas_sostav.way_num, bas_sostav.id_station_kis).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_ways;
                }

                // Формирование общего списка вагонов и постановка их на путь станции прибытия (строку корректируе основной)
                IBufferArrivalSostav ibas = bas_sostav;
                int res_set_list = SetListWagon(ref ibas);
                bas_sostav = (RWBufferArrivalSostav)ibas;
                if (bas_sostav.list_no_set_wagons == null & bas_sostav.list_wagons == null) return 0;

                if (res_set_list >= 0)
                {
                    List<int> set_wagons = new List<int>();
                    // Обнавляем вагоны
                    if (bas_sostav.count_set_wagons != null & bas_sostav.list_no_set_wagons != null)
                    {
                        set_wagons = GetWagonsToListInt(bas_sostav.list_no_set_wagons); // доствавим вагоны
                    }
                    // Ставим вагоны в первый раз
                    if (bas_sostav.count_set_wagons == null & bas_sostav.list_no_set_wagons == null & bas_sostav.list_wagons != null)
                    {
                        set_wagons = GetWagonsToListInt(bas_sostav.list_wagons); // поставим занаво
                    }
                    if (set_wagons.Count() == 0) return 0;
                    ResultTransfers result_first = new ResultTransfers(set_wagons.Count(), 0, null, null, 0, 0);
                    // Ставим вагоны на путь станции
                    bas_sostav.list_no_set_wagons = null;
                    foreach (int wag in set_wagons)
                    {
                        if (result_first.SetResultInsert(SetCarToWayRailWay(bas_sostav.natur, wag, bas_sostav.datetime, id_ways_rw)))
                        {
                            // Ошибка
                            bas_sostav.list_no_set_wagons += wag.ToString() + ";";
                        }
                    }
                    bas_sostav.count_set_wagons = bas_sostav.count_set_wagons == null ? result_first.ResultInsert : (int)bas_sostav.count_set_wagons + result_first.ResultInsert;
                    mess_arr_sostav += mess_transf1 + String.Format("По данным системы КИС, определено для переноса: {0} вагонов, перенесено: {1} вагонов, ранее перенесено: {2} вагонов, ошибок переноса {3}.",
                        set_wagons.Count(), result_first.inserts, result_first.skippeds, result_first.errors);
                    mess_arr_sostav.WriteInformation(servece_owner, eventID);
                    if (set_wagons.Count() > 0) { mess_arr_sostav.WriteEvents(result_first.errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                    // Сохранить результат и вернуть код
                    if (ef_tkis.SaveRWBufferArrivalSostav(bas_sostav) < 0)
                    { return (int)errorTransfer.set_table_arrival_sostav; }
                    else { return result_first.ResultInsert; }
                }
                else
                {
                    return res_set_list; // вернуло ошибку
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetWayRailWayOfKIS(orc_sostav={0})", bas_sostav.GetFieldsAndValue()), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// Создать и поставить на путь входящий вагон по данным КИС таблица PromVagon
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num_vag"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_way"></param>
        /// <param name="id_station_kis"></param>
        /// <returns></returns>
        public int SetCarKISToWayRailWay(int natur, int num_vag, DateTime dt_amkr, int id_way, int id_station_kis)
        {
            try
            {
                EFWagons ef_wag = new EFWagons();
                RWOperation rw_oper = new RWOperation(this.servece_owner);

                // Получим информацию для заполнения вагона с учетом отсутствия данных в PromVagon
                Prom_Vagon pv = GetCorrectVagon(natur, num_vag, dt_amkr, id_station_kis);
                Prom_NatHist pnh = GetCorrectNatHist(natur, num_vag, dt_amkr, id_station_kis);
                if (pv == null & pnh == null) return (int)errorTransfer.no_wagon_is_list;   // Ошибка нет вагонов в списке
                if (pv == null)
                {
                    pv = ef_wag.CreateProm_Vagon(pnh);
                }
                // Создать новый вагон по данным КИС 
                int res_car = 0;
                Cars new_car = SetCarsToRailWay(pv, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_amkr, dt_amkr, natur, null, pv.GODN));
                if (new_car != null)
                {
                    res_car = rw_oper.SaveChanges(new_car);
                }
                return res_car;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarKISToWayRailWay(natur={0}, num_vag={1}, dt_amkr={2}, id_way={3}, id_station_kis={4})",natur,num_vag,dt_amkr,id_way,id_station_kis), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// Поставить вагон на путь станции системы RailWay
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num_vag"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_way"></param>
        /// <returns></returns>
        public int SetCarToWayRailWay(int natur, int num_vag, DateTime dt_amkr, int id_way)
        {
            try
            {
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                EFRailWay ef_rw = new EFRailWay();
                EFMetallurgTrans ef_mt = new EFMetallurgTrans();
                //EFWagons ef_wag = new EFWagons();

                Ways way = ef_rw.GetWays(id_way);
                Stations station = way.Stations;

                // Получим станции УЗ по которым можно получать вагоны на указаную станцию
                List<Stations> list_station_arrival_uz_to_station = ef_rw.GetArrivalStationsNodes(station.id).Where(s => s.Stations.station_uz == true).ToList().Select(s => s.Stations).ToList();
                // Получить список путей станций УЗ по которым можно получать вагоны на указаную станцию
                List<Ways> list_ways_arrival_uz_to_station = new List<Ways>();
                list_station_arrival_uz_to_station.ForEach(s => list_ways_arrival_uz_to_station.Add(ef_rw.GetWaysOfArrivalUZ(s.id)));
                // Получить список путей отправки и приема
                List<Ways> list_arrival_ways_uz = ef_rw.GetWays().Where(w => w.Stations.station_uz == true & w.num == "1").ToList();
                List<Ways> list_sending_ways_uz = ef_rw.GetWays().Where(w => w.Stations.station_uz == true & w.num == "2").ToList();
                // Получим последнюю открытую операцию по указанному вагону
                CarOperations last_operation = rw_oper.GetLastOpenOperation(rw_oper.IsOpenAllOperationOfNum(num_vag), true); // проверить вагон в системе 
                if (last_operation != null)
                {   // Вагон есть в системе 
                    Cars car = last_operation.Cars; // Определим вагон
                    // Вагон статит на пути станции УЗ (по которой возможен проход вагона на станцию АМКР) для отправки на станцию АМКР?
                    int id_way_car = last_operation.IsSetWay(list_ways_arrival_uz_to_station.Select(w => w.id).ToArray(), null);
                    if (id_way_car > 0)
                    { // Стоит в прибытии станции УЗ по которой можно получить вагон на станцию АМКР
                        //TODO: Дополнительно можно сделать проверку на интервал времени нахождения в прибытии
                        //if (car.dt_uz >= dt_amkr.AddDays(this.day_waiting_interval_cars * -1))

                        //car.dt_inp_amkr = dt_amkr;
                        //car.natur_kis = natur;
                        // Выполним операцию
                        //int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_amkr, dt_amkr, natur, null, pv.GODN));
                        int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_amkr, dt_amkr, natur, null, null));
                        if (res_car > 0)
                        {
                            // Закрываем прибытие
                            int res_close_mt = ef_mt.CloseArrivalCars(car.id_sostav, car.num, natur, dt_amkr);
                        }
                        Console.WriteLine("[RailWay] Вагон {0} - cтоит в прибытии станции УЗ по которой можно получить вагон на станцию АМКР - результат переноса {1}", num_vag, res_car);
                        return res_car;

                    }
                    else
                    { // Не стоит в прибытии станции УЗ по которой можно получить вагон на станцию АМКР
                        // Вагон статит на пути любой станции УЗ для отправки на станцию АМКР?
                        id_way_car = last_operation.IsSetWay(list_arrival_ways_uz.Select(w => w.id).ToArray(), null);
                        if (id_way_car > 0)
                        { // Стоит в прибытии с любой станции УЗ 

                            //car.dt_inp_amkr = dt_amkr;
                            //car.natur_kis = natur;
                            // Выполним операцию
                            //int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_amkr, dt_amkr, natur, null, pv.GODN));
                            int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_amkr, dt_amkr, natur, null, null));
                            if (res_car > 0)
                            {
                                // Закрываем прибытие
                                int res_close_mt = ef_mt.CloseArrivalCars(car.id_sostav, car.num, natur, dt_amkr);
                            }
                            Console.WriteLine("[RailWay] Вагон {0} - cтоит в прибытии станции УЗ - результат переноса {1}", num_vag, res_car);
                            return res_car;

                        }
                        else
                        { // Не стоит в прибытии со всех станций УЗ
                            // Вагон статит на пути принятия с АМКР, любой станции УЗ?
                            id_way_car = last_operation.IsSetWay(list_sending_ways_uz.Select(w => w.id).ToArray(), null);
                            if (id_way_car > 0)
                            { // Вагон отправлен на УЗ назад
                                if ((car.dt_uz >= dt_amkr.AddDays(this.day_waiting_interval_cars * -1) & (car.natur_kis == null)) | (car.natur_kis != null && car.natur_kis == natur))
                                { // Вагон во временом деапазоне и натурка KIS неопределена

                                    //car.dt_inp_amkr = dt_amkr;
                                    //car.natur_kis = natur;
                                    // Выполним операцию
                                    //int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_amkr, dt_amkr, natur, null, pv.GODN));
                                    int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_amkr, dt_amkr, natur, null, null));
                                    if (res_car > 0)
                                    {
                                        // Закрываем прибытие
                                        int res_close_mt = ef_mt.CloseArrivalCars(car.id_sostav, car.num, natur, dt_amkr);
                                    }
                                    Console.WriteLine("[RailWay] Вагон {0} - cтоит на станции УЗ принятый с АМКР - результат переноса {1}", num_vag, res_car);
                                    return res_car;

                                }
                                else
                                {
                                    // Закрыть старый
                                    int res_car_close = rw_oper.CloseSaveCar(car, dt_amkr, true);
                                    // Создать новый вагон по данным КИС 
                                    int res_car = 0;
                                    if (res_car_close > 0)
                                    {
                                        res_car = SetCarKISToWayRailWay(natur, num_vag, dt_amkr, id_way, (int)station.id_kis);
                                    }
                                    Console.WriteLine("[RailWay] Вагон {0} - cтоит на станции УЗ принятый с АМКР уже давно - результат закрытия старого {1}. Создать новый вагон по данным КИС  - результат создания {2}", num_vag, res_car_close, res_car);
                                    return res_car;
                                }

                            }
                            else
                            { // Вагон находится в системе RAILWAY

                                //Console.WriteLine("Вагон {0} - cтоит в сисстеме Railway", car.num);
                                if (car.dt_uz >= dt_amkr.AddDays(this.day_waiting_interval_cars * -1) & (car.natur_kis == null || (car.natur_kis != null & car.natur_kis == natur)))
                                {
                                    // Вагон находится во временном диапазоне и натурка не определена или равна
                                    // Обновим
                                    int res_car = rw_oper.SetSaveCar(car, natur, null, dt_amkr);
                                    if (res_car > 0)
                                    {
                                        // Закрываем прибытие
                                        int res_close_mt = ef_mt.CloseArrivalCars(car.id_sostav, car.num, natur, dt_amkr);
                                    }
                                    Console.WriteLine("[RailWay] Вагон {0} - cтоит в сисстеме Railway, находится во временном диапазоне и натурка не определена или равна - результат обновления {1}", num_vag, res_car);
                                    return res_car;

                                }
                                else
                                {
                                    // Вагон не находится во временном диапазоне или натурка определена и неравна текущей
                                    // Закрыть старый
                                    int res_car_close = rw_oper.CloseSaveCar(car, dt_amkr, true);
                                    int res_car = 0;
                                    if (res_car_close > 0)
                                    {
                                        res_car = SetCarKISToWayRailWay(natur, num_vag, dt_amkr, id_way, (int)station.id_kis);
                                    }
                                    // Создать вагон по данным КИС
                                    Console.WriteLine("[RailWay] Вагон {0} - cтоит в сисстеме Railway, НЕ находится во временном диапазоне или натурка пределена и НЕ равна - результат закрытия старого {1}. Создать новый вагон по данным КИС - результат создания {2}", num_vag, res_car_close, res_car);
                                    return res_car;
                                }

                            }
                        }
                    }
                }
                else
                {
                    // Вагона небыло в системе 
                    // Создать вагон по данным КИС
                    int res_car = 0;
                    res_car = SetCarKISToWayRailWay(natur, num_vag, dt_amkr, id_way, (int)station.id_kis);
                    Console.WriteLine("[RailWay] Вагон {0} - НЕ было в сисстеме RAILWAY. Создать новый вагон по данным КИС - результат создания {1}", num_vag, res_car);
                    return res_car;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarToWayRailWay(natur={0}, num_vag={1}, dt_amkr={2}, id_way={3})",natur,num_vag,dt_amkr,id_way), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        #endregion

        #region ОБНОВИТЬ ВАГОН НА ПУТЬ ПРИЕМА НА АМКР СИСТЕМЫ RAILWAY ПО ДАННЫМ NAT.HIST
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bas_sostav"></param>
        /// <returns></returns>
        public int UpdWayRailWayOfKIS(ref RWBufferArrivalSostav bas_sostav)
        {
            try
            {
                string mess_upd = String.Format("состава (натурный лист: {0}, дата: {1}, ID строки буфера переноса: {2}), прибывшего на АМКР", bas_sostav.natur, bas_sostav.datetime, bas_sostav.id);
                string mess_upd_sostav = "[RailWay] Обновление " + mess_upd;
                string mess_error_upd_sostav = "[RailWay] Ошибка обновления";

                RWReference rw_ref = new RWReference(base.servece_owner, reference_kis); // создавать содержимое справочника из данных КИС
                EFWagons ef_wag = new EFWagons();
                EFTKIS ef_tkis = new EFTKIS();

                if (bas_sostav == null) return 0;
                if (bas_sostav.count_set_wagons == null) return 0; // вагоны не поставлены на путь станции

                int id_stations_rw = 0;
                int id_ways_rw = 0;

                id_stations_rw = rw_ref.GetIDStationsOfKIS(bas_sostav.id_station_kis);
                if (id_stations_rw <= 0)
                {
                    String.Format(mess_error_upd_sostav + mess_upd + " - ID станции: {0} не определён в справочнике системы RailWay", bas_sostav.id_station_kis).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_stations;
                }
                if (id_stations_rw == 26) id_stations_rw = 27; // Коррекция Промышленная Керамет -> 'это промышленная
                // Определим путь на станции система RailCars
                id_ways_rw = rw_ref.GetIDDefaultWayOfStation(id_stations_rw, bas_sostav.way_num.ToString());
                if (id_ways_rw <= 0)
                {
                    String.Format(mess_error_upd_sostav + mess_upd + " - ID пути: {0} станции: {1} не определён в справочнике системы RailWay", bas_sostav.way_num, bas_sostav.id_station_kis).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_ways;
                }

                // Обновим информацию по количеству вагонов в таблице NatHist
                //List<PromNatHist> list_nh = ef_wag.GetNatHistPR(bas_sostav.natur, bas_sostav.id_station_kis, bas_sostav.day, bas_sostav.month, bas_sostav.year, bas_sostav.napr == 2 ? true : false).ToList();
                List<Prom_NatHist> list_nh = ef_wag.GetArrivalProm_NatHistOfNaturStationDate(bas_sostav.natur, bas_sostav.id_station_kis, bas_sostav.day, bas_sostav.month, bas_sostav.year, bas_sostav.napr == 2 ? true : false).ToList();
                if (list_nh== null || list_nh.Count()==0) {
                    DateTime next_date = new DateTime(bas_sostav.year, bas_sostav.month, bas_sostav.day).AddDays(1); // Следующая дата (натурка водится в 23:00 а вагон принимается на следующий день) 
                    list_nh = ef_wag.GetArrivalProm_NatHistOfNaturStationDate(bas_sostav.natur, bas_sostav.id_station_kis, next_date.Day, next_date.Month, next_date.Year, bas_sostav.napr == 2 ? true : false).ToList();
                    if (list_nh == null || list_nh.Count() == 0) { 
                        next_date = new DateTime(bas_sostav.year, bas_sostav.month, bas_sostav.day).AddDays(-1); // Следующая дата (иногда тупят и дату ставят раньше чем приняли вагон)     
                        list_nh = ef_wag.GetArrivalProm_NatHistOfNaturStationDate(bas_sostav.natur, bas_sostav.id_station_kis, next_date.Day, next_date.Month, next_date.Year, bas_sostav.napr == 2 ? true : false).ToList();
                    }
                }
                DateTime dt_pr = bas_sostav.datetime;
                // Проверим 
                if (list_nh == null || list_nh.Count() == 0)
                {
                    bas_sostav.count_nathist = null;
                }
                else
                {
                    bas_sostav.count_nathist = list_nh.Count();
                    if ((DateTime)list_nh[0].DT_PR!= null && dt_pr != (DateTime)list_nh[0].DT_PR)
                    {
                        dt_pr = (DateTime)list_nh[0].DT_PR;
                        String.Format(mess_upd_sostav + mess_upd + " - не совпадают даты прибытия состава {0} и вагонов {1}, будет произведена корректировка даты и времени прибытия", bas_sostav.datetime, list_nh[0].DT_PR).WriteWarning(servece_owner, eventID);
                    }
                }
                 //= list_nh.Count() > 0 ? list_nh.Count() as int? : null;

                List<int> set_wagons = new List<int>();
                // Обнавляем вагоны
                if (bas_sostav.count_set_nathist != null & bas_sostav.list_no_update_wagons != null & bas_sostav.count_set_wagons != null)
                {
                    set_wagons = GetWagonsToListInt(bas_sostav.list_no_update_wagons); // до обновим вагоны
                }
                if (bas_sostav.count_set_nathist != null & bas_sostav.list_no_update_wagons == null & bas_sostav.count_set_wagons != null & bas_sostav.count_set_wagons != bas_sostav.count_set_nathist)
                {
                    set_wagons = GetWagonsToListInt(bas_sostav.list_wagons); // поставим занаво
                }
                // Обновляем вагоны в первый раз
                if ((bas_sostav.count_set_nathist == null | bas_sostav.count_set_nathist == 0) & bas_sostav.list_no_update_wagons == null & bas_sostav.count_set_wagons != null)
                {
                    set_wagons = GetWagonsToListInt(bas_sostav.list_wagons); // поставим занаво
                }
                if (set_wagons.Count() == 0) return 0;
                ResultTransfers result = new ResultTransfers(set_wagons.Count(), null, 0, null, 0, 0);
                bas_sostav.list_no_update_wagons = null;
                bas_sostav.message = null;
                // Ставим вагоны на путь станции
                foreach (int wag in set_wagons)
                {
                    if (result.SetResultUpdate(UpdCarToWayRailWay(bas_sostav.natur, wag, dt_pr, id_ways_rw)))
                    {
                        // Ошибка
                        bas_sostav.list_no_update_wagons += wag.ToString() + ";";
                        bas_sostav.message += wag.ToString() + ":" + result.result.ToString() + ";";
                    }
                }
                bas_sostav.count_set_nathist = (bas_sostav.count_set_nathist != null ? bas_sostav.count_set_nathist : 0) + result.ResultUpdate;
                mess_upd_sostav += String.Format(" (id_rw_station : {0}, id_rw_way : {1}) по данным системы КИС. Определено для обновления: {2} вагонов, обновлено: {3} вагонов, ранее обновлено: {4} вагонов, ошибок обновления {5}.", id_stations_rw, id_ways_rw, set_wagons.Count(), result.updates, result.skippeds, result.errors);
                mess_upd_sostav.WriteInformation(servece_owner, eventID);
                //if (set_wagons.Count() > 0) { mess_update_sostav.SaveLogEvents(result.errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                // Сохранить результат и вернуть код
                if (ef_tkis.SaveRWBufferArrivalSostav(bas_sostav) < 0)
                { return (int)errorTransfer.set_table_arrival_sostav; }
                else { return result.ResultUpdate; }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("UpdWayRailWayOfKIS(bas_sostav={0})", bas_sostav.GetFieldsAndValue()), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }

        public int UpdCarToWayRailWay(int natur, int num_vag, DateTime dt_amkr, int id_way)
        {
            try
            {
                //RWOperation rw_oper = new RWOperation(this.servece_owner);
                EFRailWay ef_rw = new EFRailWay();
                RWReference rw_ref = new RWReference(base.servece_owner, reference_kis);
                //EFMetallurgTrans ef_mt = new EFMetallurgTrans();
                //EFWagons ef_wag = new EFWagons();

                int result = 0;

                Ways way = ef_rw.GetWays(id_way);
                Stations station = way.Stations;

                string mess = String.Format("грузополучателя и годности вагона №:{0}, принадлежащего составу (натурный лист: {1}, дата: {2}) стоящего на пути станции (станция АМКР: {3}, путь: {4})", num_vag, natur, dt_amkr, station.name_ru, way.num + "- " + way.name_ru);
                string mess_update_vag = "[RailWay] Обновление " + mess;
                string mess_update_vag_err = "[RailWay] Ошибка обновления " + mess;

                Prom_NatHist pnh = GetCorrectNatHist(natur, num_vag, dt_amkr, (int)station.id_kis);
                if (pnh == null)
                {
                    String.Format(mess_update_vag_err + ", код ошибки:{0}", errorTransfer.no_wagon_is_nathist.ToString()).WriteWarning(servece_owner, eventID);
                    return (int)errorTransfer.no_wagon_is_nathist;
                }
                // Определим грузополучателя
                int? id_consignee = null;
                if (pnh.K_POL_GR != null)
                {
                    id_consignee = rw_ref.GetIDReferenceConsigneeOfKis((int)pnh.K_POL_GR, true);
                }

                // Обновим данные
                if (id_consignee != null | pnh.GODN != null | !String.IsNullOrWhiteSpace(pnh.ST_OTPR))
                {
                    Cars car = ef_rw.GetCarsOfSetKIS(num_vag, dt_amkr, natur);
                    if (car != null)
                    {
                        if (id_consignee != null) car.CarsInpDelivery.ToList()[0].id_consignee = id_consignee;
                        if (!String.IsNullOrWhiteSpace(pnh.ST_OTPR)) car.CarsInpDelivery.ToList()[0].station_shipment = pnh.ST_OTPR;
                        if (pnh.GODN != null)
                        {
                            foreach (CarOperations operations in car.CarOperations)
                            {
                                if (operations.id_car_conditions == null) operations.id_car_conditions = pnh.GODN;
                            }
                        }
                        result = ef_rw.SaveCars(car);
                        if (result < 0)
                        {
                            String.Format(mess_update_vag_err + ", код ошибки:{0}", result).WriteError(servece_owner, eventID);
                            return result;
                        }
                    }
                }
                if (id_consignee == null)
                {
                    String.Format(mess_update_vag_err + ", код ошибки:{0}, PromNatHist.K_POL_GR:{1}.", errorTransfer.no_shop.ToString(), pnh.K_POL_GR).WriteWarning(servece_owner, eventID);
                    return (int)errorTransfer.no_shop;
                }
                //Обновлять пока не появится годность
                if (pnh.GODN == null)
                {
                    String.Format(mess_update_vag_err + ", код ошибки:{0}", errorTransfer.no_godn.ToString()).WriteWarning(servece_owner, eventID);
                    return (int)errorTransfer.no_godn;
                }
                return result;

            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("UpdCarToWayRailWay(natur={0}, num_vag={1}, dt_amkr={2}, id_way={3})", natur, num_vag, dt_amkr, id_way), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        #endregion

        #region ПОСТАВИМ НА ПУТЬ СДАЧИ НА УЗ СИСТЕМЫ RAILWAY ПО ДАННЫМ NAT.HIST
        /// <summary>
        /// Оновим список поставленных и не поставленных вагонов
        /// </summary>
        /// <param name="bss_sostav"></param>
        /// <param name="list_nh"></param>
        /// <returns></returns>
        public int SetListWagon(ref RWBufferSendingSostav bss_sostav, List<Prom_NatHist> list_nh)
        {
            EFTKIS ef_tkis = new EFTKIS();
            if (list_nh == null || list_nh.Count() == 0)
                return (int)errorTransfer.no_list_nathist; // Списков вагонов нет
            try
            {

                //Создать и список вагонов заново и поставить их на путь
                List<int> old_wagons = GetWagonsToListInt(bss_sostav.list_wagons);
                bss_sostav.list_wagons = GetWagonsToString(list_nh);
                List<int> new_wagons = new List<int>();
                new_wagons = GetWagonsToListInt(bss_sostav.list_wagons);

                List<int> wagons_no_set = GetWagonsToListInt(bss_sostav.list_no_set_wagons);

                List<int> wagons_buf = new List<int>();
                List<int> wagons_no_set_buf = new List<int>();

                // Удалить вагоны не найденные в новом списке из списка непоставленных на станцию вагонов 
                if (wagons_no_set != null)
                {
                    wagons_buf = GetWagonsToListInt(bss_sostav.list_wagons);
                    wagons_no_set_buf = GetWagonsToListInt(bss_sostav.list_no_set_wagons);
                    DeleteExistWagon(ref wagons_buf, ref wagons_no_set_buf);
                    foreach (int wag in wagons_no_set_buf)
                    {
                        DeleteExistWagon(ref wagons_no_set, wag);
                    }
                }
                // сформировать строчные списки не поставленных и не обнавленных вагонов
                bss_sostav.list_no_set_wagons = GetWagonsToString(wagons_no_set);
                // Добавить в списки не поставленных и не обнавленных вагонов новые вагоны из нового списка
                DeleteExistWagon(ref new_wagons, ref old_wagons);
                foreach (int wag in new_wagons)
                {
                    if (wagons_no_set != null)
                    { bss_sostav.list_no_set_wagons += wag.ToString() + ";"; }
                }

                bss_sostav.count_nathist = list_nh.Count();
                return ef_tkis.SaveRWBufferSendingSostav(bss_sostav);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetListWagon(bss_sostav={0}, list_nh={1})", bss_sostav.GetFieldsAndValue(), list_nh), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }

        public int SetWayRailWayOfKIS(ref RWBufferSendingSostav bss_sostav)
        {
            try
            {
                string mess_transf = String.Format("cостава (натурный лист: {0}, дата: {1}, ID строки буфера переноса: {2}), отправленный с АМКР на УЗ",
                    bss_sostav.natur, bss_sostav.datetime, bss_sostav.id);
                string mess_transf1 = " по данным системы КИС.";
                string mess_arr_sostav = "[RailWay] Перенос " + mess_transf;
                string mess_error_arr_sostav = "[RailWay] Ошибка переноса " + mess_transf;

                RWReference rw_ref = new RWReference(base.servece_owner, reference_kis); // создавать содержимое справочника из данных КИС
                EFWagons ef_wag = new EFWagons();
                EFTKIS ef_tkis = new EFTKIS();

                int id_stations_amkr = 0;
                int id_stations_uz = 0;

                id_stations_amkr = rw_ref.GetIDStationsOfKIS(bss_sostav.id_station_from_kis);
                if (id_stations_amkr <= 0)
                {
                    String.Format(mess_error_arr_sostav + mess_transf1 + " - ID станции АМКР: {0} не определён в справочнике системы RailWay", bss_sostav.id_station_from_kis).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_stations;
                }
                if (id_stations_amkr == 26) id_stations_amkr = 27; // Коррекция Промышленная Керамет -> 'это промышленная
                id_stations_uz = rw_ref.GetIDStationsOfKIS(bss_sostav.id_station_on_kis);
                if (id_stations_uz <= 0)
                {
                    String.Format(mess_error_arr_sostav + mess_transf1 + " - ID станции УЗ: {0} не определён в справочнике системы RailWay", bss_sostav.id_station_on_kis).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_stations;
                }
                int id_way_uz = 0;
                // Определим путь на станции система RailCars
                id_way_uz = rw_ref.GetIDDefaultWayOfStation(id_stations_uz, "2");
                if (id_way_uz <= 0)
                {
                    String.Format(mess_error_arr_sostav + mess_transf1 + " - ID пути: {0} станции УЗ: {1} не определён в справочнике системы RailWay", "2", bss_sostav.id_station_on_kis).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_ways;
                }
                // Получим список вагонов
                List<Prom_NatHist> list_nh = ef_wag.GetSendingProm_NatHistOfNaturDateTime(bss_sostav.natur,
                    bss_sostav.day,
                    bss_sostav.month,
                    bss_sostav.year,
                    bss_sostav.hour,
                    bss_sostav.minute,
                    false).ToList();
                // Проверим есть список вагонов по указанной натурке дате\месяцу\году\часу\минутах
                if (list_nh == null || list_nh.Count() == 0)
                {
                    list_nh = ef_wag.GetSendingProm_NatHistOfNaturDate(bss_sostav.natur,
                                        bss_sostav.day,
                                        bss_sostav.month,
                                        bss_sostav.year,
                                        false).ToList();
                    if (list_nh == null || list_nh.Count() == 0)
                    {
                        // Проверим есть список вагонов по указанной натурке дате\месяцу\году
                        list_nh = ef_wag.GetSendingProm_NatHistOfNaturDate(bss_sostav.natur,
                                            bss_sostav.day + 1,
                                            bss_sostav.month,
                                            bss_sostav.year,
                                            false).ToList();
                    }
                    if (list_nh != null && list_nh.Count() > 0)
                    {
                        // Скорректируем дату отправки
                        bss_sostav.datetime = (DateTime)list_nh[0].DT_SD;
                        ef_tkis.SaveRWBufferSendingSostav(bss_sostav);
                    }

                }
                // Определим списки вагонов
                int res_set_list = SetListWagon(ref bss_sostav, list_nh);
                if (res_set_list < 0)
                {
                    // вагонов для переноса по данным КИС - нет
                    bss_sostav.message = ((errorTransfer)res_set_list).ToString();
                    ef_tkis.SaveRWBufferSendingSostav(bss_sostav);
                    return res_set_list; // вернуло ошибку
                }
                else
                {
                    // Получены списки вагонов для переноса
                    if (bss_sostav.list_no_set_wagons == null & bss_sostav.list_wagons == null)
                    {
                        // вагонов для переноса по данным КИС - нет
                        bss_sostav.message = errorTransfer.no_list_wagons.ToString();
                        ef_tkis.SaveRWBufferSendingSostav(bss_sostav);
                        return (int)errorTransfer.no_list_wagons; // вернуло ошибку                    
                    }

                    List<int> set_wagons = new List<int>();
                    // Обнавляем вагоны
                    if (bss_sostav.count_set_nathist != null & bss_sostav.list_no_set_wagons != null)
                    {
                        set_wagons = GetWagonsToListInt(bss_sostav.list_no_set_wagons); // доствавим вагоны
                    }
                    // Ставим вагоны в первый раз
                    if (bss_sostav.count_set_nathist == null & bss_sostav.list_no_set_wagons == null & bss_sostav.list_wagons != null)
                    {
                        set_wagons = GetWagonsToListInt(bss_sostav.list_wagons); // поставим занаво
                    }
                    if (set_wagons.Count() == 0) return 0;
                    ResultTransfers result_set_way = new ResultTransfers(set_wagons.Count(), 0, null, null, 0, 0);
                    // Ставим вагоны на путь станции
                    bss_sostav.list_no_set_wagons = null;
                    bss_sostav.message = null;
                    foreach (int wag in set_wagons)
                    {
                        if (result_set_way.SetResultInsert(SetCarToSendingWayRailWay(bss_sostav.natur, wag, bss_sostav.datetime, id_way_uz, id_stations_amkr)))
                        {
                            // Ошибка
                            bss_sostav.list_no_set_wagons += wag.ToString() + ";";
                            bss_sostav.message += wag.ToString() + ":" + result_set_way.result + ";";
                        }
                    }
                    bss_sostav.count_set_nathist = bss_sostav.count_set_nathist == null ? result_set_way.ResultInsert : (int)bss_sostav.count_set_nathist + result_set_way.ResultInsert;
                    mess_arr_sostav += mess_transf1 + String.Format("По данным системы КИС, определено для переноса на путь станции УЗ: {0} вагонов, перенесено: {1} вагонов, ранее перенесено: {2} вагонов, ошибок переноса {3}.",
                        set_wagons.Count(), result_set_way.inserts, result_set_way.skippeds, result_set_way.errors);
                    mess_arr_sostav.WriteInformation(servece_owner, eventID);
                    if (set_wagons.Count() > 0) { mess_arr_sostav.WriteEvents(result_set_way.errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                    // Сохранить результат и вернуть код
                    if (ef_tkis.SaveRWBufferSendingSostav(bss_sostav) < 0)
                    { return (int)errorTransfer.set_table_sending_sostav; }
                    else { return result_set_way.ResultInsert; }
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetWayRailWayOfKIS(orc_sostav={0})", bss_sostav.GetFieldsAndValue()), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// Сдать вагон на "Путь для приема с АМКР" станции КР УЗ
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num_vag"></param>
        /// <param name="dt_out_amkr"></param>
        /// <param name="id_sending_way"></param>
        /// <param name="id_station_amkr"></param>
        /// <returns></returns> 
        public int SetCarToSendingWayRailWay(int natur, int num_vag, DateTime dt_out_amkr, int id_sending_way, int id_station_amkr)
        {
            try
            {
                string mess = String.Format("вагона {0} на путь {1} станции УЗ Кривого Рога, станция отправления АМКР {2}, натурный лист {3}, дата и время {4}.",
                    num_vag, id_sending_way, id_station_amkr, natur, dt_out_amkr);
                string mess_ok = "[RailWay] Отправка " + mess;
                string mess_err = "[RailWay] Ошибка при отправке" + mess;

                EFWagons ef_wag = new EFWagons();
                EFRailWay ef_rw = new EFRailWay();

                // Определим входящие данные по таблице Prom.NatHist
                //List<PromNatHist> list = ef_wag.GetNatHistOfVagonLessPR(num_vag, dt_out_amkr, true).ToList();
                Prom_NatHist pnh_arrival = ef_wag.GetArrivalProm_NatHistOfVagonLess(num_vag, dt_out_amkr, true).FirstOrDefault();
                if (pnh_arrival != null)
                {
                    // Входящие данные есть, определим вагон
                    Cars car = ef_rw.GetCarsOfSetKIS(pnh_arrival.N_VAG, (DateTime)pnh_arrival.DT_PR, pnh_arrival.N_NATUR);
                    if (car == null)
                    {
                        car = ef_rw.GetCarsOfSetKIS(pnh_arrival.N_VAG, (DateTime)pnh_arrival.DT_PR, pnh_arrival.N_NATUR, this.day_waiting_interval_cars * 24);
                    }
                    if (car == null)
                    {
                        // Вагона нет в системе как принятого на АМКР
                        // Ищем вагон без даты захода на АМКР но с указанной натуркой
                        car = ef_rw.GetCarsOfSetKIS(pnh_arrival.N_VAG, pnh_arrival.N_NATUR);
                        if (car == null)
                        {
                            // Вагона нет в системе без даты захода на АМКР но с указанной натуркой
                            // Ищем вагон без даты захода на АМКР без натурки но стоящий на УЗ с интервалом времени 24 часа
                            car = ef_rw.GetCarsOfNumDT(pnh_arrival.N_VAG, (DateTime)pnh_arrival.DT_PR, this.day_waiting_interval_cars * 24);
                            if (car == null)
                            {
                                // Создать на пути прибытия и вернуть id_car
                                int res_create_car = SetCarUZArrivalToRW(pnh_arrival);
                                // Ошибка или операция не выполнилась
                                if (res_create_car < 0) return res_create_car;
                                car = ef_rw.GetCars(res_create_car);
                            }
                        }
                        else
                        {
                            // Обновить информацию о заходе вагона на АМКР
                            int res_upd_car = UpdInpDelivery(car.id, (DateTime)pnh_arrival.DT_PR, pnh_arrival.N_NATUR);
                            // Ошибка или операция не выполнилась
                            if (res_upd_car < 0) return res_upd_car;
                        }

                    }
                    // Вагон есть в системе
                    // По id_car ставим вагон на УЗ и если есть следующий входящий вагон закрываем предыдущий
                    int res = SetCarToSendingWayRailWay(car.id, natur, dt_out_amkr, id_sending_way, pnh_arrival);
                    if (log_detali)
                    {
                        if (res > 0)
                        {
                            (mess_ok + String.Format(" Вагон перенесен, код выполнения {0} ", res)).WriteInformation(servece_owner, this.eventID);
                        }
                        if (res == 0)
                        {
                            (mess_ok + String.Format(" Вагон пропущен, код выполнения {0} ", res)).WriteInformation(servece_owner, this.eventID);
                        }
                        if (res < 0)
                        {
                            (mess_err + String.Format(" Код ошибки {0} ", res)).WriteInformation(servece_owner, this.eventID);
                        }
                    }
                    return res;
                }
                else
                {
                    // Входящих данных НЕТ.
                    mess_err += String.Format(" В системе КИС нет данных по прибытию вагона на АМКР, код ошибки {0}.", errorTransfer.no_wagon_is_nathist);
                    mess_err.WriteError(servece_owner, this.eventID);
                    return (int)errorTransfer.no_wagon_is_nathist;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarToSendingWayRailWay(natur={0}, num_vag={1}, dt_out_amkr={2}, id_sending_way={3}, id_station_amkr={4})", natur, num_vag, dt_out_amkr, id_sending_way, id_station_amkr), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_car"></param>
        /// <param name="natur_out"></param>
        /// <param name="dt_out_amkr"></param>
        /// <param name="id_sending_way"></param>
        /// <param name="pnh_arrival"></param>
        /// <returns></returns>
        public int SetCarToSendingWayRailWay(int id_car, int natur_out, DateTime dt_out_amkr, int id_sending_way, Prom_NatHist pnh_arrival)
        {

            try
            {
                string mess = String.Format("вагона id_car:{0} на путь {1} станции УЗ Кривого Рога, натурный лист отправки {2}, дата и время отправки {3}.",
                    id_car, id_sending_way, natur_out, dt_out_amkr);
                string mess_ok = "[RailWay] Отправка " + mess;
                string mess_err = "[RailWay] Ошибка при отправке" + mess;

                EFRailWay ef_rw = new EFRailWay();
                Cars car = ef_rw.GetCars(id_car);
                if (car == null) return 0;
                //-----------------------------------------------------
                // Вагон стоит на пути "Принятия с АМКР"станций УЗ 
                // получить список путей "Принятия с АМКР" станций УЗ 
                List<Ways> list_sending_ways_uz = ef_rw.GetWays().Where(w => w.Stations.station_uz == true & w.num == "2").ToList();
                CarOperations last_operation = car.GetLastOperations();
                int id_way_car_set = last_operation.IsSetWay(list_sending_ways_uz.Select(w => w.id).ToArray(), null);
                int id_way_car_pass = last_operation.IsPassWay(list_sending_ways_uz.Select(w => w.id).ToArray(), null);
                if (id_way_car_set > 0 | id_way_car_pass > 0)
                {
                    // Вагон стоит на пути "Принятия с АМКР" станции УЗ 
                    if ((car.natur_kis_out != null & car.natur_kis_out == natur_out) | (car.natur_kis_out == null))
                    {
                        // Обновим CarsOutDelivery, natur_kis_out, dt_out_amkr
                        int res_upd_car = UpdOutDelivery(car.id, dt_out_amkr, natur_out);
                        // Ошибка или операция не выполнилась
                        if (res_upd_car < 0) return res_upd_car;
                        // выход из if продолжим

                    }
                    else
                    {
                        // Ошибка, выход
                        bool res = IsVagonOfSendingNatHistNatur((int)car.natur_kis_out, car.num, (DateTime)car.dt_out_amkr);
                        if (res)
                        {
                            mess_err += String.Format(" Натурные листы отправки системы КИС:{0} и RW:{1}- отличаются.", natur_out, car.natur_kis_out);
                            mess_err.WriteError(servece_owner, this.eventID);
                            return (int)errorTransfer.different_nanur_out;
                        }
                        else { 
                            // По указаной натурке вагона уже нет (были сделаны корректировки грузовой службой)
                            mess_ok += String.Format(" Натурные листы отправки системы КИС:{0} и RW:{1}- отличаются. Будет произведена замена.", natur_out, car.natur_kis_out);
                            mess_ok.WriteWarning(servece_owner, this.eventID);                            
                            // Удалим старую операцию
                            int res_del_operation = DeleteLastOperationSaveCar(car.id);
                            // Ошибка или операция не выполнилась
                            if (res_del_operation < 0) return res_del_operation;
                            // Обновим CarsOutDelivery и сдадим на УЗ
                            int res_sending_car = SetCarSendingUZToRW(car.id, natur_out, dt_out_amkr, id_sending_way);
                            // Ошибка или операция не выполнилась
                            if (res_sending_car < 0) return res_sending_car;
                            // выход из if
                        }

                    }
                    // тогда выход из if
                }
                else
                {
                    // Вагон НЕ стоит на пути "Принятия с АМКР" станции УЗ
                    //----------------------------------------------------
                    // Вагон стоит на АМКР?
                    List<Ways> list_all_ways_station_amkr = new List<Ways>();
                    list_all_ways_station_amkr = ef_rw.GetWaysOfStationAMKR().ToList();
                    id_way_car_set = last_operation.IsSetWay(list_all_ways_station_amkr.Select(w => w.id).ToArray(), null);
                    id_way_car_pass = last_operation.IsPassWay(list_all_ways_station_amkr.Select(w => w.id).ToArray(), null);
                    if (id_way_car_set > 0 | id_way_car_pass > 0)
                    {
                        // Вагон стоит на путях станций АМКР
                        // Обновим CarsOutDelivery и сдадим на УЗ
                        int res_sending_car = SetCarSendingUZToRW(car.id, natur_out, dt_out_amkr, id_sending_way);
                        // Ошибка или операция не выполнилась
                        if (res_sending_car < 0) return res_sending_car;
                        // выход из if
                    }
                    else
                    {
                        // Вагон НЕ стоит на путях станций АМКР
                        //----------------------------------------------------
                        // Вагон стоит на путях "Прибытия на АМКР" станций УЗ?
                        List<Ways> list_arrival_ways_uz = ef_rw.GetWays().Where(w => w.Stations.station_uz == true & w.num == "1").ToList();
                        id_way_car_set = last_operation.IsSetWay(list_arrival_ways_uz.Select(w => w.id).ToArray(), null);
                        id_way_car_pass = last_operation.IsPassWay(list_arrival_ways_uz.Select(w => w.id).ToArray(), null);
                        if (id_way_car_set > 0 | id_way_car_pass > 0)
                        {
                            //Вагон стоит на путях "Прибытия на АМКР" станций УЗ
                            // Принять на АМКР
                            int res_arrival_car = SetCarArrivalUZToRW(car.id, pnh_arrival);
                            // Ошибка или операция не выполнилась
                            if (res_arrival_car < 0) return res_arrival_car;
                            // Обновим CarsOutDelivery и сдадим на УЗ
                            int res_sending_car = SetCarSendingUZToRW(car.id, natur_out, dt_out_amkr, id_sending_way);
                            // Ошибка или операция не выполнилась
                            if (res_sending_car < 0) return res_sending_car;
                            // выход из if
                        }
                        else
                        {
                            // Ошибка, выход
                        }
                    }
                }
                // Операция выполнена id_car обновлен
                return CloseCarSendingUZToRW(car.id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarToSendingWayRailWay(id_car={0}, natur_out={1}, dt_out_amkr={2}, id_sending_way={3}, pnh_arrival={4})",
                    id_car, natur_out, dt_out_amkr, id_sending_way, pnh_arrival.GetFieldsAndValue()));
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// Принять вагон на АМКР с путей "для отправки на АМКР" станции УЗ
        /// </summary>
        /// <param name="id_car"></param>
        /// <param name="pnh"></param>
        /// <returns></returns>
        public int SetCarArrivalUZToRW(int id_car, Prom_NatHist pnh)
        {
            try
            {
                if (pnh == null) return 0;
                RWReference rw_ref = new RWReference(base.servece_owner, reference_kis); // создавать содержимое справочника из данных КИС
                int id_stations_rw = rw_ref.GetIDStationsOfKIS(pnh.K_ST);
                if (id_stations_rw <= 0)
                {
                    return (int)errorTransfer.no_stations;
                }
                if (id_stations_rw == 26) id_stations_rw = 27; // Коррекция Промышленная Керамет -> 'это промышленная
                // Определим путь на станции система RailCars
                int id_ways_rw = rw_ref.GetIDDefaultWayOfStation(id_stations_rw, null);
                if (id_ways_rw <= 0)
                {
                    return (int)errorTransfer.no_ways;
                }
                return SetCarArrivalUZToRW(id_car, pnh.N_NATUR, (DateTime)pnh.DT_PR, id_ways_rw);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarArrivalUZToRW(id_car={0}, pnh={1})", id_car, pnh.GetFieldsAndValue()), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// Принять вагон на АМКР с путей "для отправки на АМКР" станции УЗ
        /// </summary>
        /// <param name="id_car"></param>
        /// <param name="natur"></param>
        /// <param name="dt_inp_amkr"></param>
        /// <param name="id_way"></param>
        /// <returns></returns>
        public int SetCarArrivalUZToRW(int id_car, int natur, DateTime dt_inp_amkr, int id_way)
        {
            try
            {
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                EFMetallurgTrans ef_mt = new EFMetallurgTrans();
                Cars car = rw_oper.GetCars(id_car);
                int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_inp_amkr, dt_inp_amkr, natur, null, null));
                if (res_car > 0)
                {
                    // Закрываем прибытие
                    int res_close_mt = ef_mt.CloseArrivalCars(car.id_sostav, car.num, natur, dt_inp_amkr);
                }
                return res_car;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarArrivalUZToRW(id_car={0}, natur={1}, dt_inp_amkr={2}, id_way={3})", id_car, natur, dt_inp_amkr, id_way), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// Сдать вагон на путь "для принятия из АМКР" станции УЗ
        /// </summary>
        /// <param name="id_car"></param>
        /// <param name="natur_out"></param>
        /// <param name="dt_out_amkr"></param>
        /// <param name="id_sending_way"></param>
        /// <returns></returns>
        public int SetCarSendingUZToRW(int id_car, int natur_out, DateTime dt_out_amkr, int id_sending_way)
        {
            try
            {
                // Исходящая поставка
                int res_upd_delivery = UpdOutDelivery(id_car, dt_out_amkr, natur_out);
                if (res_upd_delivery < 0) return res_upd_delivery;
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                Cars car = rw_oper.GetCars(id_car);
                int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationSendingUZWay, new OperationSendingUZWay(id_sending_way, dt_out_amkr, dt_out_amkr, natur_out, null, null));
                if (res_car > 0)
                {

                }
                return res_car;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarSendingUZToRW(id_car={0}, natur_out={1}, dt_out_amkr={2}, id_sending_way={3})", id_car, natur_out, dt_out_amkr, id_sending_way), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// Обновить исходящую поставку
        /// </summary>
        /// <param name="id_car"></param>
        /// <param name="dt_out_amkr"></param>
        /// <returns></returns>
        public int UpdOutDelivery(int id_car, DateTime dt_out_amkr, int? natur_kis_out)
        {
            try
            {
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                Cars car = rw_oper.GetCars(id_car);
                car.SetCar(dt_out_amkr, natur_kis_out);
                // Исходящая поставка
                CarsOutDelivery delivery = CreateCarsOutDelivery(car.num, dt_out_amkr);
                return rw_oper.SetSaveCar(car, delivery);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("UpdOutDelivery(id_car={0}, dt_out_amkr={1}, natur_kis_out={2})", id_car, dt_out_amkr, natur_kis_out), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// Обновить входящие поставки
        /// </summary>
        /// <param name="id_car"></param>
        /// <param name="dt_inp_amkr"></param>
        /// <param name="natur_kis_inp"></param>
        /// <returns></returns>
        public int UpdInpDelivery(int id_car, DateTime dt_inp_amkr, int? natur_kis_inp)
        {
            try
            {
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                Cars car = rw_oper.GetCars(id_car);
                return rw_oper.SetSaveCar(car, natur_kis_inp, null, dt_inp_amkr);
                // входящая поставка
                //CarsOutDelivery delivery = CreateCarsOutDelivery(car.num, dt_out_amkr);
                //return rw_oper.SetSaveCar(car, delivery);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("UpdInpDelivery(id_car={0}, dt_inp_amkr={1}, natur_kis_inp={2})", id_car, dt_inp_amkr, natur_kis_inp), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// Проверим наличие следущего "Входящего вагона", если есть закроем старый
        /// </summary>
        /// <param name="id_car"></param>
        /// <returns></returns>
        public int CloseCarSendingUZToRW(int id_car)
        {
            try
            {
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                Cars next_car = rw_oper.GetNextCars(id_car);
                if (next_car != null)
                {
                    Cars car = rw_oper.GetCars(id_car);
                    // Закроем car
                    return rw_oper.CloseSaveCar(car, (DateTime)next_car.dt_uz, true);
                }
                return id_car;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseCarSendingUZToRW(id_car={0})", id_car), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// Удалить последнюю операцию открыть предпоследнюю
        /// </summary>
        /// <param name="id_car"></param>
        /// <returns></returns>
        public int DeleteLastOperationSaveCar(int id_car)
        {
            RWOperation rw_oper = new RWOperation(this.servece_owner);
            return rw_oper.DeleteLastOperationSaveCar(id_car);
        }
        /// <summary>
        /// Поставить вагон на путь "для отправки на АМКР" станции УЗ
        /// </summary>
        /// <param name="pnh_arrival"></param>
        /// <returns></returns>
        public int SetCarUZArrivalToRW(Prom_NatHist pnh_arrival)
        {
            try
            {
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                EFMetallurgTrans ef_mt = new EFMetallurgTrans();
                EFRailWay ef_rw = new EFRailWay();
                RWReference rw_ref = new RWReference(base.servece_owner, reference_kis); // создавать содержимое справочника из данных КИС

                //Cars car = null;
                CarsInpDelivery delivery = null;
                Stations station = null;
                // Найдем вапгон в прибытии с УЗ КР (по данным метттранс)
                ArrivalCars arr_car = ef_mt.GetArrivalCarsToNatur(pnh_arrival.N_NATUR, pnh_arrival.N_VAG, (DateTime)pnh_arrival.DT_PR, 15);
                if (arr_car != null)
                {
                    station = rw_ref.GetStationsUZ(arr_car.CompositionIndex, true);
                    delivery = rw_oper.CreateCarsInpDelivery(arr_car);
                }
                else
                {
                    //TODO: Можно предусматреть доп проверку на прибытие по данным метттранс
                    station = rw_ref.GetStations((int)pnh_arrival.K_ST_OTPR, true);
                    delivery = CreateCarsInpDelivery(pnh_arrival);
                }
                int id_way = rw_ref.GetIDWayOfStation(station.id, "1");

                // Создадим новый "Входящий вагон"
                int res_create_car = SetCarUZArrivalToRW(pnh_arrival.N_VAG,
                    (arr_car != null ? arr_car.IDSostav : (pnh_arrival.N_NATUR * -1)),
                    delivery,
                    rw_oper.OperationSetWay,
                    new OperationSetWay(id_way, arr_car != null ? arr_car.Position : (int)pnh_arrival.NPP, arr_car != null ? arr_car.DateOperation : (DateTime)pnh_arrival.DT_PR, 15, null));
                return res_create_car;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarUZArrivalToRW(pnh_arrival={0})", pnh_arrival.GetFieldsAndValue()), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Поставить вагон на путь "для отправки на АМКР" станции УЗ
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id_sostav"></param>
        /// <param name="delivery"></param>
        /// <param name="set_operation"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public int SetCarUZArrivalToRW(int num, int id_sostav, CarsInpDelivery delivery, RW.RWOperation.SetCarOperation set_operation, IOperation operation)
        {
            try
            {
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                EFMetallurgTrans ef_mt = new EFMetallurgTrans();
                RWReference rw_ref = new RWReference(base.servece_owner, reference_kis); // создавать содержимое справочника из данных КИС

                List<Cars> list_cars = rw_oper.GetCarsOfNum(num).OrderBy(c => c.dt_uz).ToList();
                int? id_car_previous = null;
                int? id_car_next = null;
                // Определим пердыдущий и следующий "Входящий вагон" (и вставим между)
                foreach (Cars car in list_cars)
                {
                    DateTime dt = (DateTime)car.dt_uz;
                    //System.TimeSpan diff1 = ((DateTime)car.dt_inp_amkr).Subtract((DateTime)car.dt_uz).Days;
                    if (car.dt_inp_amkr != null && ((DateTime)car.dt_inp_amkr).Subtract((DateTime)car.dt_uz).TotalHours > 12)
                    {
                        dt = (DateTime)car.dt_inp_amkr;
                    }

                    if (dt < operation.dt_set)
                    {
                        id_car_previous = car.id;
                    }
                    else
                    {
                        if (id_car_next == null)
                        {
                            id_car_next = car.id;
                        }
                    }
                }
                // Создаем входящий вагон

                // Проверим наличие вагона в справочнике если нет создадим + если есть из КИС перенесем аренды и владельца
                ReferenceCars ref_car = rw_ref.GetReferenceCarsOfNum(num, delivery.id_arrival, operation.dt_set, (int)delivery.id_country, true, true);
                // Создадим строку 
                Cars new_car = new Cars()
                {
                    id = 0,
                    id_arrival = delivery.id_arrival,
                    num = num,
                    dt_inp_amkr = null,
                    dt_out_amkr = null,
                    natur = null,
                    natur_kis = operation.natur_kis,
                    parent_id = id_car_previous,
                };
                if (this.log_detali)
                {
                    //(mess += String.Format(" - в системе RailWay создана новая запись 'Входящего вагона'")).WriteInformation(servece_owner, eventID);
                }

                // Входяшие поставки
                if (new_car.CarsInpDelivery == null || new_car.CarsInpDelivery.Count() == 0)
                { new_car.CarsInpDelivery.Add(delivery); }
                new_car.id_sostav = id_sostav;
                new_car.dt_uz = operation.dt_set;
                // Закрываем прибытие (по вагону пришло ТСП, а вагон уже принят на АМКР)
                if (new_car.natur != null | new_car.natur_kis != null)
                {
                    int res_close_mt = ef_mt.CloseArrivalCars(new_car.id_sostav, new_car.num, new_car.natur != null ? (int)new_car.natur : (int)new_car.natur_kis, (DateTime)new_car.dt_uz);
                }
                // Создаем новую операцию
                CarOperations oper = set_operation(new_car, operation);
                int res_create_car = rw_oper.SaveChanges(new_car);
                // Ошибка или операция не выполнилась
                if (res_create_car < 0) return res_create_car;
                // переопределить потомка в следуещем "Входящем вагоне" 
                if (id_car_next != null & res_create_car > 0)
                {
                    Cars car_next = rw_oper.GetCars((int)id_car_next);
                    car_next.parent_id = res_create_car;
                    int res_parent = rw_oper.SaveChanges(car_next);
                    if (res_parent < 0)
                    {
                        String.Format("Метод [SetCarUZArrivalToRW]. Ошибка обновления поля parent_id={0}, строки 'Входящий вагон' id_car={1}", res_create_car, id_car_next).WriteError(servece_owner, this.eventID);
                    }
                }
                // закрыть операции в предыдущем "Входящем вагоне" 
                if (id_car_previous != null & res_create_car > 0)
                {
                    int res_previous = ClosePreviousCar((int)id_car_previous, operation.dt_set);
                    if (res_previous < 0)
                    {
                        String.Format("Метод [SetCarUZArrivalToRW]. Ошибка закрытия строки 'Входящий вагон' id_car={0}, закрывал следующий 'Входящий вагон' id_car={1}, дата закрытия {2}", res_previous, res_create_car, operation.dt_set).WriteError(servece_owner, this.eventID);
                    }
                }
                return res_create_car;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarUZArrivalToRW(delivery={0})", delivery.GetFieldsAndValue()), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Закрыть предыдущий входящий вагон (с проверкой если вагон принят на АМКР но не закрыть - закрыть)
        /// </summary>
        /// <param name="id_car"></param>
        /// <param name="close"></param>
        /// <returns></returns>
        public int ClosePreviousCar(int id_car, DateTime close)
        {
            try
            {
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                RWReference rw_ref = new RWReference(base.servece_owner, reference_kis); // создавать содержимое справочника из данных КИС

                Cars car_previous = rw_oper.GetCars(id_car);
                if (car_previous == null) return 0;
                if (car_previous.parent_id != null && car_previous.dt_uz != null)
                {
                    ClosePreviousCar((int)car_previous.parent_id, (DateTime)car_previous.dt_uz);
                }
                // Проверим в "Входящем вагоне" который хотим закрыть наличие входящего и исходящего натурного листа КИС 
                if (car_previous.natur_kis != null && car_previous.natur_kis_out == null)
                {
                    EFWagons ef_wag = new EFWagons();
                    // Найдем в КИС отправленный вагон на УЗ
                    //List<PromNatHist> list = ef_wag.GetNatHistOfVagonMoreSD(car_previous.num, (DateTime)car_previous.dt_inp_amkr, false).ToList();
                    //PromNatHist pnh_sending = ef_wag.GetNatHistOfVagonMoreSD(car_previous.num, (DateTime)car_previous.dt_inp_amkr, false).FirstOrDefault();
                    //List<Prom_NatHist> list = ef_wag.GetProm_NatHistOfVagonMoreSD(car_previous.num, (DateTime)car_previous.dt_inp_amkr, false).ToList();
                    Prom_NatHist pnh_sending = ef_wag.GetSendingProm_NatHistOfVagonMore(car_previous.num, (DateTime)car_previous.dt_inp_amkr, false).FirstOrDefault();

                    if (pnh_sending != null)
                    {
                        int id_stations_uz = rw_ref.GetIDStationsOfKIS((int)pnh_sending.K_ST_NAZN);
                        int id_way_uz = rw_ref.GetIDDefaultWayOfStation(id_stations_uz, "2");
                        // Вагон стоит на путях станций АМКР
                        // Обновим CarsOutDelivery и сдадим на УЗ
                        int res_sending_car = SetCarSendingUZToRW(car_previous.id, pnh_sending.N_NATUR, (DateTime)pnh_sending.DT_SD, id_way_uz);

                    }
                }
                return rw_oper.CloseSaveCar(car_previous, close, true);

            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("ClosePreviousCar(id_car={0}, close={1})", id_car, close), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }

        #endregion

        #region ПЕРЕНОС ВАГОНОВ ПО ВНУТРЕНИМ СТАНЦИЯМ ПО ПРИБЫТИЮ
        /// <summary>
        /// Копирование истории в таблицу состояния переноса составов (перенос по прибытию) RWBufferInputSostav
        /// </summary>
        /// <returns></returns>
        public int CopyBufferInputSostavOfKIS()
        {
            return CopyBufferInputSostavOfKIS(this.day_control_input_kis_add_data);
        }
        /// <summary>
        /// Копирование истории в таблицу состояния переноса составов (перенос по прибытию) RWBufferInputSostav
        /// </summary>
        /// <param name="day_control_ins"></param>
        /// <returns></returns>
        public int CopyBufferInputSostavOfKIS(int day_control_ins)
        {
            EFTKIS ef_tkis = new EFTKIS();
            EFWagons ef_wag = new EFWagons();
            int errors = 0;
            int normals = 0;
            // список новых составов в системе КИС
            List<NumVag_Stpr1InStDoc> list_newsostav = new List<NumVag_Stpr1InStDoc>();
            // список уже перенесенных в RailWay составов в системе КИС (с учетом периода контроля dayControllingAddNatur)
            List<NumVag_Stpr1InStDoc> list_oldsostav = new List<NumVag_Stpr1InStDoc>();
            // список уже перенесенных в RailWay составов (с учетом периода контроля dayControllingAddNatur)
            List<RWBufferInputSostav> list_inputsostav = new List<RWBufferInputSostav>();
            try
            {
                // Считаем дату последненго состава
                DateTime? lastDT = ef_tkis.GetLastDateTimeRWBufferInputSostav();
                if (lastDT != null)
                {
                    // Данные есть получим новые
                    list_newsostav = ef_wag.GetNumVag_Stpr1InStDoc(((DateTime)lastDT).AddSeconds(1), DateTime.Now, false).ToList();
                    list_oldsostav = ef_wag.GetNumVag_Stpr1InStDoc(((DateTime)lastDT).AddDays(day_control_ins * -1), ((DateTime)lastDT).AddSeconds(1), false).ToList();
                    list_inputsostav = ef_tkis.GetRWBufferInputSostav(((DateTime)lastDT).AddDays(day_control_ins * -1), ((DateTime)lastDT).AddSeconds(1)).ToList();
                }
                else
                {
                    // Таблица пуста получим первый раз
                    list_newsostav = ef_wag.GetNumVag_Stpr1InStDoc(DateTime.Now.AddDays(day_control_ins * -1), DateTime.Now, false).ToList();
                }
                // Переносим информацию по новым составам
                if (list_newsostav.Count() > 0)
                {
                    foreach (NumVag_Stpr1InStDoc inps in list_newsostav)
                    {

                        int res = SaveRWBufferInputSostav(inps, statusSting.Normal);
                        if (res > 0) normals++;
                        if (res < 1) { errors++; }
                    }
                    string mess_new = String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Таблица состояния переноса составов (по прибытию), по данным системы КИС (определено новых составов:{0}, перенесено:{1}, ошибок переноса:{2}).", list_newsostav.Count(), normals, errors);
                    mess_new.WriteInformation(servece_owner, this.eventID);
                    if (list_newsostav.Count() > 0) mess_new.WriteEvents(errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID);
                }
                // Обновим информацию по составам которые были перенесены
                if (list_oldsostav.Count() > 0 & list_inputsostav.Count() > 0)
                {
                    List<NumVag_Stpr1InStDoc> list_is = new List<NumVag_Stpr1InStDoc>();
                    list_is = list_oldsostav;
                    List<RWBufferInputSostav> list_ois = new List<RWBufferInputSostav>();
                    list_ois = list_inputsostav.Where(a => a.status != (int)statusSting.Delete).ToList();
                    DelExistRWBufferInputSostav(ref list_is, ref list_ois);
                    int ins = InsertRWBufferInputSostav(list_is);
                    int del = DeleteRWBufferInputSostav(list_ois);

                    string mess_upd = String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Таблица состояния переноса сосставов (по прибытию) по данным системы КИС (определено добавленных составов:{0}, перенесено:{1}, определено удаленных составов:{2}, удалено:{3}).",
                    list_is.Count(), ins, list_ois.Count(), del);
                    mess_upd.WriteInformation(servece_owner, this.eventID);
                    if (list_is.Count() > 0 | list_is.Count() > 0) mess_upd.WriteEvents(EventStatus.Ok, servece_owner, eventID);
                    normals += ins;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CopyBufferInputSostavOfKIS(day_control_ins={0})", day_control_ins), servece_owner, eventID);
                return -1;
            }
            return normals;
        }

        public int TransferInputSostavKISToRailway()
        {
            try
            {
                EFTKIS ef_tkis = new EFTKIS();
                int close = 0;
                IQueryable<RWBufferInputSostav> list_noClose = ef_tkis.GetRWBufferInputSostavNoClose();
                if (list_noClose == null || list_noClose.Count() == 0) return 0;
                foreach (RWBufferInputSostav bis in list_noClose.ToList())
                {
                    RWBufferInputSostav kis_inp_sostav = new RWBufferInputSostav();
                    kis_inp_sostav = bis;
                    if (TransferInputSostavKISToRailway(ref kis_inp_sostav) > 0)
                    {
                        close++;
                    }
                }
                return close;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferInputSostavKISToRailway()"), servece_owner, eventID);
                return -1;
            }
        }

        public int TransferInputSostavKISToRailway(ref RWBufferInputSostav bis)
        {
            try
            {
                EFRW.Concrete.EFRailWay ef_rw = new EFRW.Concrete.EFRailWay();

                // Обновим количество вагонов
                int res_list_wagons = UpdateCountWagons(ref bis);
                if (res_list_wagons < 0)
                {
                    return (int)errorTransfer.error_list_wagons;
                }
                //Закрыть состав
                int res_close = 0;
                res_close = CheckCloseInputSostavKIS(ref bis);
                if (res_close > 0)
                {
                    return res_close;
                }
                // Проверка соответствия переносу и правилам
                if (this.type_transfer_input_kis == 1 ||
                    (this.type_transfer_input_kis == 2 && ef_rw.IsRulesTransferOfKis(bis.id_station_from_kis, bis.id_station_on_kis, EFRW.Concrete.EFRailWay.typeSendTransfer.kis_input)))
                {
                    // переносим
                    int res_put = SetInputSostavKISToRailway(ref bis);
                    if (res_put >= 0)
                    {
                        // Если перенесли, закроем состав
                        res_close = CheckCloseInputSostavKIS(ref bis);
                    }
                }
                else
                {
                    // Пропустить и закрыть состав
                    res_close = CloseInputSostavKIS(ref bis, "Пропущен и закрыт, " + (this.type_transfer_output_kis == 0 ? "сервис переноса отключен [TypeTransferInputKis=" + this.type_transfer_input_kis + "]." : "перенос не соответствует правилам."));
                }
                return res_close;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferInputSostavKISToRailway(bis={0})", bis.GetFieldsAndValue()), servece_owner, eventID);
                return -1;
            }
        }

        public int CheckCloseInputSostavKIS(ref RWBufferInputSostav bis)
        {
            int res_close = 0;
            //Закрыть состав
            if (bis.count_wagons != null & bis.count_set_wagons != null & bis.count_wagons == bis.count_set_wagons)
            {
                res_close = CloseInputSostavKIS(ref bis, "Перенесен и закрыт.");
            }
            return res_close;
        }
        /// <summary>
        /// Закрыть строку таблицы состояния переноса с информационным сообщением
        /// </summary>
        /// <param name="bis"></param>
        /// <param name="text_close"></param>
        /// <returns></returns>
        public int CloseInputSostavKIS(ref RWBufferInputSostav bis, string text_close)
        {
            int res_close = 0;
            bis.close_comment = text_close;
            res_close = CloseInputSostavKIS(ref bis);
            string mess_put = String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Состав, натурная ведомость: {0} от {1} по прибытию из внутренних станций, по данным системы КИС, ID строки - {2}", bis.doc_num, bis.datetime, bis.id);
            mess_put += " - " + text_close;
            mess_put.WriteEvents(res_close > 0 ? EventStatus.Ok : EventStatus.Error, servece_owner, eventID);
            return res_close;
        }
        /// <summary>
        /// Закрыть строку таблицы состояния переноса
        /// </summary>
        /// <param name="bis"></param>
        /// <returns></returns>
        public int CloseInputSostavKIS(ref RWBufferInputSostav bis)
        {
            EFTKIS ef_tkis = new EFTKIS();
            int res_close = 0;
            bis.close = DateTime.Now;
            bis.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
            res_close = ef_tkis.SaveRWBufferInputSostav(bis);
            return res_close;
        }

        public int SetInputSostavKISToRailway(ref RWBufferInputSostav bis)
        {

            RWReference rw_ref = new RWReference(base.servece_owner, reference_kis); // создавать содержимое справочника из данных КИС
            EFWagons ef_wag = new EFWagons();

            string mess_transf = String.Format("состава (№ документа: {0}, дата: {1}, ID строки: {2}) по прибытию из внутренних станций по данным системы КИС", bis.doc_num, bis.datetime, bis.id);
            string mess_sostav_err = "[Учёт ж.д. вагонов на внутренних станциях АМКР]. Ошибка переноса " + mess_transf;
            // Определим станцию отправитель
            int? id_stations_from = rw_ref.GetIDStationsOfKIS(bis.id_station_from_kis);
            if (id_stations_from == null)
            {
                String.Format(mess_sostav_err + " - ID станции отправки: {0} не определён в справочнике системы RailWay", bis.id_station_from_kis).WriteError(servece_owner, eventID);
                return (int)errorTransfer.no_stations;
            }
            if (id_stations_from == 26) id_stations_from = 27; // Коррекция Промышленная Керамет -> 'это промышленная

            // Определим станцию получатель
            int? id_stations_on = rw_ref.GetIDStationsOfKIS(bis.id_station_on_kis);
            if (id_stations_on == null)
            {
                String.Format(mess_sostav_err + " - ID станции прибытия: {0} не определён в справочнике системы RailWay", bis.id_station_on_kis).WriteError(servece_owner, eventID);
                return (int)errorTransfer.no_stations;
            }
            if (id_stations_from == 26) id_stations_from = 27; // Коррекция Промышленная Керамет -> 'это промышленная

            //// Определим путь на станции
            //int? id_ways = rw_ref.DefinitionIDWays((int)id_stations_from);
            //if (id_ways == null)
            //{
            //    String.Format(mess_sostav_err + " - ID пути: {0} станции: {1} не определён в справочнике системы RailWay", bos.way_num_kis, id_stations_from).WriteError(servece_owner, eventID);
            //    return (int)errorTransfer.no_ways;
            //}
            // Формирование общего списка вагонов и постановка их на путь станции прибытия
            //List<NumVag_Stpr1OutStVag> list_car = ef_wag.GetNumVag_Stpr1OutStVag(bos.doc_num, bos.napr == 2 ? true : false).ToList();

            //bos.count_wagons = list_car.Count(); // Определим количество вагонов
            //return TransferArrivalSostavToStation(ref bos, (int)id_stations_from, (int)id_stations_on, (int)id_ways);
            return 0;
        }
        #endregion

        #region ПЕРЕНОС ВАГОНОВ ПО ВНУТРЕНИМ СТАНЦИЯМ ПО ОТПРАВКЕ
        /// <summary>
        /// Копирование истории в таблицу состояния переноса составов (перенос по отправке) RWBufferOutputSostav
        /// </summary>
        /// <returns></returns>
        public int CopyBufferOutputSostavOfKIS()
        {
            return CopyBufferOutputSostavOfKIS(this.day_control_output_kis_add_data, this.status_control_output_kis);
        }
        /// <summary>
        /// Копирование истории в таблицу состояния переноса составов (перенос по отправке) RWBufferOutputSostav
        /// </summary>
        /// <param name="day_control_ins"></param>
        /// <returns></returns>
        public int CopyBufferOutputSostavOfKIS(int day_control_ins, bool is_status)
        {
            EFTKIS ef_tkis = new EFTKIS();
            EFWagons ef_wag = new EFWagons();
            int errors = 0;
            int normals = 0;
            // список новых составов в системе КИС
            List<NumVag_Stpr1OutStDoc> list_newsostav = new List<NumVag_Stpr1OutStDoc>();
            // список уже перенесенных в RailWay составов в системе КИС (с учетом периода контроля dayControllingAddNatur)
            List<NumVag_Stpr1OutStDoc> list_oldsostav = new List<NumVag_Stpr1OutStDoc>();
            // список уже перенесенных в RailWay составов (с учетом периода контроля dayControllingAddNatur)
            List<RWBufferOutputSostav> list_outputsostav = new List<RWBufferOutputSostav>();
            try
            {
                // Считаем дату последненго состава
                DateTime? lastDT = ef_tkis.GetLastDateTimeRWBufferOutputSostav();
                if (lastDT != null)
                {
                    // Данные есть получим новые
                    list_newsostav = ef_wag.GetNumVag_Stpr1OutStDoc(((DateTime)lastDT).AddSeconds(1), DateTime.Now, false).ToList();
                    list_oldsostav = ef_wag.GetNumVag_Stpr1OutStDoc(((DateTime)lastDT).AddDays(day_control_ins * -1), ((DateTime)lastDT).AddSeconds(1), false).ToList();
                    list_outputsostav = ef_tkis.GetRWBufferOutputSostav(((DateTime)lastDT).AddDays(day_control_ins * -1), ((DateTime)lastDT).AddSeconds(1)).ToList();
                }
                else
                {
                    // Таблица пуста получим первый раз
                    list_newsostav = ef_wag.GetNumVag_Stpr1OutStDoc(DateTime.Now.AddDays(day_control_ins * -1), DateTime.Now, false).ToList();
                }
                // Переносим информацию по новым составам
                if (list_newsostav.Count() > 0)
                {
                    foreach (NumVag_Stpr1OutStDoc inps in list_newsostav)
                    {
                        if (is_status && inps.STATUS != 1) break;
                        int res = SaveRWBufferOutputSostav(inps, statusSting.Normal);
                        if (res > 0) normals++;
                        if (res < 1) { errors++; }

                    }
                    string mess_new = String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Таблица состояния переноса составов (по отправке), по данным системы КИС (определено новых составов:{0}, перенесено:{1}, ошибок переноса:{2}).", list_newsostav.Count(), normals, errors);
                    mess_new.WriteInformation(servece_owner, this.eventID);
                    if (list_newsostav.Count() > 0) mess_new.WriteEvents(errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID);
                }
                // Обновим информацию по составам которые были перенесены
                if (list_oldsostav.Count() > 0 & list_outputsostav.Count() > 0)
                {
                    List<NumVag_Stpr1OutStDoc> list_os = new List<NumVag_Stpr1OutStDoc>();
                    list_os = list_oldsostav;
                    List<RWBufferOutputSostav> list_oos = new List<RWBufferOutputSostav>();
                    list_oos = list_outputsostav.Where(a => a.status != (int)statusSting.Delete).ToList();
                    DelExistRWBufferOutputSostav(ref list_os, ref list_oos);
                    int ins = InsertRWBufferOutputSostav(list_os);
                    int del = DeleteRWBufferOutputSostav(list_oos);
                    string mess_upd = String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Таблица состояния переноса сосставов (по отправке) по данным системы КИС (определено добавленных составов:{0}, перенесено:{1}, определено удаленных составов:{2}, удалено:{3}).",
                        list_os.Count(), ins, list_oos.Count(), del);
                    mess_upd.WriteInformation(servece_owner, this.eventID);
                    if (list_os.Count() > 0 | list_os.Count() > 0) mess_upd.WriteEvents(EventStatus.Ok, servece_owner, eventID);
                    normals += ins;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CopyBufferOutputSostavOfKIS(day_control_ins={0}, is_status={1})", day_control_ins, is_status), servece_owner, eventID);
                return -1;
            }

            return normals;
        }
        /// <summary>
        /// Перенос составов по отправке по данным таблицы буффера переносов (с проверкой правил переноса).
        /// </summary>
        /// <returns></returns>
        public int TransferOutputSostavKISToRailway() {
            try
            {
                EFTKIS ef_tkis = new EFTKIS();
                int close = 0;
                IQueryable<RWBufferOutputSostav> list_noClose = ef_tkis.GetRWBufferOutputSostavNoClose();
                if (list_noClose == null || list_noClose.Count() == 0) return 0;
                foreach (RWBufferOutputSostav bos in list_noClose.ToList())
                {
                    RWBufferOutputSostav kis_out_sostav = new RWBufferOutputSostav();
                    kis_out_sostav = bos;
                    if (TransferOutputSostavKISToRailway(ref kis_out_sostav) > 0)
                    {
                        close++;
                    }
                }
                return close;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferOutputSostavKISToRailway()"), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Перенос состава по отправке по данным таблицы буффера переносов (с проверкой правил переноса).
        /// </summary>
        /// <param name="bos"></param>
        /// <returns></returns>
        public int TransferOutputSostavKISToRailway(ref RWBufferOutputSostav bos) {
            try
            {
                EFRW.Concrete.EFRailWay ef_rw = new EFRW.Concrete.EFRailWay();

                // Обновим количество вагонов
                int res_list_wagons = UpdateCountWagons(ref bos);
                if (res_list_wagons < 0)
                {
                    return (int)errorTransfer.error_list_wagons;
                }
                //Закрыть состав
                int res_close = 0;
                res_close = CheckCloseOutputSostavKIS(ref bos);
                if (res_close > 0)
                {
                    return res_close;
                }
                // Проверка соответствия переносу и правилам
                if (this.type_transfer_output_kis == 1 ||
                    (this.type_transfer_output_kis == 2 && ef_rw.IsRulesTransferOfKis(bos.id_station_from_kis, bos.id_station_on_kis, EFRW.Concrete.EFRailWay.typeSendTransfer.kis_output)))
                {
                    // переносим
                    int res_put = SetOutputSostavKISToRailway(ref bos);
                    if (res_put >= 0)
                    {
                        // Если перенесли, закроем состав
                        res_close = CheckCloseOutputSostavKIS(ref bos);
                    }
                }
                else
                {
                    // Пропустить и закрыть состав
                    res_close = CloseOutputSostavKIS(ref bos, "Пропущен и закрыт, " + (this.type_transfer_output_kis == 0 ? "сервис переноса отключен [TypeTransferOutputKis=" + this.type_transfer_output_kis + "]." : "перенос не соответствует правилам."));
                }
                return res_close;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferOutputSostavKISToRailway(bos={0})", bos.GetFieldsAndValue()), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Проверить и закрыть строку таблицы состояния переноса
        /// </summary>
        /// <param name="bos"></param>
        /// <returns></returns>
        public int CheckCloseOutputSostavKIS(ref RWBufferOutputSostav bos)
        {
            int res_close = 0;
            //Закрыть состав
            if (bos.count_wagons != null & bos.count_set_wagons != null & bos.count_wagons == bos.count_set_wagons)
            {
                res_close = CloseOutputSostavKIS(ref bos, "Перенесен и закрыт.");
            }
            return res_close;
        }
        /// <summary>
        /// Закрыть строку таблицы состояния переноса с информационным сообщением
        /// </summary>
        /// <param name="bos"></param>
        /// <param name="text_close"></param>
        /// <returns></returns>
        public int CloseOutputSostavKIS(ref RWBufferOutputSostav bos, string text_close)
        {
            int res_close = 0;
            bos.close_comment = text_close;
            res_close = CloseOutputSostavKIS(ref bos);
            string mess_put = String.Format("[Учёт ж.д. вагонов на внутренних станциях АМКР]. Состав, натурная ведомость: {0} от {1} на сдачу на внутренних станциях по данным системы КИС, ID строки таблицы состояния переноса - {2}", bos.doc_num, bos.datetime, bos.id);
            mess_put += " - " + text_close;
            mess_put.WriteEvents(res_close > 0 ? EventStatus.Ok : EventStatus.Error, servece_owner, eventID);
            return res_close;
        }
        /// <summary>
        /// Закрыть строку таблицы состояния переноса
        /// </summary>
        /// <param name="bos"></param>
        /// <returns></returns>
        public int CloseOutputSostavKIS(ref RWBufferOutputSostav bos)
        {
            EFTKIS ef_tkis = new EFTKIS();
            int res_close = 0;
            bos.close = DateTime.Now;
            bos.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
            res_close = ef_tkis.SaveRWBufferOutputSostav(bos);
            return res_close;
        }
        ///// <summary>
        ///// Определить список вагонов
        ///// </summary>
        ///// <param name="bos"></param>
        ///// <returns></returns>
        //public int SetCountWagons(ref RWBufferOutputSostav bos)
        //{
        //    try
        //    {
        //        int result = 0;
        //        EFWagons ef_wag = new EFWagons();
        //        EFTKIS ef_tkis = new EFTKIS();
        //        // Получить список вагонов для переноса
        //        List<NumVag_Stpr1OutStVag> list_car = ef_wag.GetNumVag_Stpr1OutStVag(bos.doc_num, bos.napr == 2 ? true : false).ToList();

        //        if (list_car != null && list_car.Count() != bos.count_wagons)
        //        {
        //            if (bos.count_wagons != null && bos.count_set_wagons != null)
        //            {
        //                // Вагоны были перенесены
        //                // Удалим вагоны из системы RailCars
        //                // TODO: Сделать код удаления вагонов из RailWay
        //            }
        //            // Определим новое количество вагонов
        //            bos.count_wagons = list_car.Count(); // Определим количество вагонов
        //            bos.list_wagons = GetWagonsToString(list_car.Select(v => v.N_VAG).ToArray().ToList());
        //            result = ef_tkis.SaveRWBufferOutputSostav(bos);
        //        }
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("SetCountWagons(bos={0})", bos.GetFieldsAndValue()), servece_owner, eventID);
        //        return (int)errorTransfer.error_list_wagons;
        //    }
        //}
        public int SetOutputSostavKISToRailway(ref RWBufferOutputSostav bos)
        {

            RWReference rw_ref = new RWReference(base.servece_owner, reference_kis); // создавать содержимое справочника из данных КИС
            EFWagons ef_wag = new EFWagons();

            string mess_transf = String.Format("состава (№ документа: {0}, дата: {1}, ID строки: {2}) по отправке из внутренних станций по данным системы КИС", bos.doc_num, bos.datetime, bos.id);
            string mess_sostav_err = "[Учёт ж.д. вагонов на внутренних станциях АМКР]. Ошибка переноса " + mess_transf;
            // Определим станцию отправитель
            int? id_stations_from = rw_ref.GetIDStationsOfKIS(bos.id_station_from_kis);
            if (id_stations_from == null)
            {
                String.Format(mess_sostav_err + " - ID станции отправки: {0} не определён в справочнике системы RailWay", bos.id_station_from_kis).WriteError(servece_owner, eventID);
                return (int)errorTransfer.no_stations;
            }
            if (id_stations_from == 26) id_stations_from = 27; // Коррекция Промышленная Керамет -> 'это промышленная

            // Определим станцию получатель
            int? id_stations_on = rw_ref.GetIDStationsOfKIS(bos.id_station_on_kis);
            if (id_stations_on == null)
            {
                String.Format(mess_sostav_err + " - ID станции прибытия: {0} не определён в справочнике системы RailWay", bos.id_station_on_kis).WriteError(servece_owner, eventID);
                return (int)errorTransfer.no_stations;
            }
            if (id_stations_from == 26) id_stations_from = 27; // Коррекция Промышленная Керамет -> 'это промышленная

            //// Определим путь на станции
            //int? id_ways = rw_ref.DefinitionIDWays((int)id_stations_from);
            //if (id_ways == null)
            //{
            //    String.Format(mess_sostav_err + " - ID пути: {0} станции: {1} не определён в справочнике системы RailWay", bos.way_num_kis, id_stations_from).WriteError(servece_owner, eventID);
            //    return (int)errorTransfer.no_ways;
            //}
            // Формирование общего списка вагонов и постановка их на путь станции прибытия
            //List<NumVag_Stpr1OutStVag> list_car = ef_wag.GetNumVag_Stpr1OutStVag(bos.doc_num, bos.napr == 2 ? true : false).ToList();

            //bos.count_wagons = list_car.Count(); // Определим количество вагонов
            //return TransferArrivalSostavToStation(ref bos, (int)id_stations_from, (int)id_stations_on, (int)id_ways);
            return 0;
        }
        #endregion


        #endregion

        #region Закрыть перенос составов
        public int CloseBufferArrivalSostav()
        {
            int res_rw = CloseRWBufferArrivalSostav();
            return res_rw;
        }

        public int CloseRWBufferArrivalSostav()
        {

            EFTKIS ef_tkis = new EFTKIS();
            int close = 0;
            int skip = 0;
            int error = 0;

            List<RWBufferArrivalSostav> list = new List<RWBufferArrivalSostav>();
            list = ef_tkis.GetRWBufferArrivalSostavNoClose().OrderBy(c => c.datetime).ToList();
            foreach (RWBufferArrivalSostav bas in list.ToList())
            {
                int res = CloseRWBufferArrivalSostav(bas);
                if (res > 0) { close++; }
                if (res == 0) { skip++; }
                if (res < 0) { error++; }
            }
            string mess = String.Format("[RailWay] Проверка буфера переноса вагонов из системы КИС в систему RailCars - выполнена, определено {0} не перенесенных состава, закрыто автоматически {1}, пропущено {2}, ошибки закрытия {3}.",
                list != null ? list.Count() : 0, close, skip, error);
            mess.WriteInformation(servece_owner, this.eventID);
            if (list != null && list.Count() > 0) { mess.WriteEvents(error > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
            return close;
        }

        public int CloseRWBufferArrivalSostav(RWBufferArrivalSostav bas)
        {

            EFWagons ef_wag = new EFWagons();
            EFTKIS ef_tkis = new EFTKIS();
            //List<PromVagon> list_pv = ef_wag.GetVagon(bas.natur, bas.id_station_kis, bas.day, bas.month, bas.year).ToList();
            List<Prom_Vagon> list_pv = ef_wag.GetArrivalProm_VagonOfNaturStationDate(bas.natur, bas.id_station_kis, bas.day, bas.month, bas.year).ToList();
            //List<PromNatHist> list_pnh = ef_wag.GetNatHistPR(bas.natur, bas.id_station_kis, bas.day, bas.month, bas.year).ToList();
            List<Prom_NatHist> list_pnh = ef_wag.GetArrivalProm_NatHistOfNaturStationDate(bas.natur, bas.id_station_kis, bas.day, bas.month, bas.year, null).ToList();
            // Ситуация-1. Проверим наличие вагонов в системе КИС (Могли отменить натурку данных нет в таблицах PromVagons, NanHist)
            if ((list_pv == null || list_pv.Count() == 0) & (list_pnh == null || list_pnh.Count() == 0))
            {
                // данных нет в двух таблицах
                //if (bas.list_wagons != null) { 
                // вагоны были выставленны
                // удалим вагоны по этому составу, но проверим если была сосздана новая натурка с этими вагонми тогда сделаем коррекцию вагонов по прибытию
                return DeleteSostavRWBufferArrivalSostav(bas.id);
                //}
            }
            // Ситуация-2.  Проверим наличие вагонов в системе КИС (Могли отменить натурку убрать данные из таблиц NanHist)
            if ((list_pv != null && list_pv.Count() > 0) & (list_pnh == null || list_pnh.Count() == 0))
            {
                return DeleteSostavRWBufferArrivalSostav(bas.id);
            }
            // Ситуация-3.  Не все вагоны обновились (нет годности и цеха))
            if ((list_pv != null && list_pv.Count() > 0) & (list_pnh != null && list_pnh.Count() > 0))
            {
                //return DeleteSostavBufferArrivalSostav(bas.id);
                if (bas.message != null)
                {
                    if (bas.message.Contains(((int)errorTransfer.no_stations).ToString())) return 0;
                    if (bas.message.Contains(((int)errorTransfer.no_ways).ToString())) return 0;
                    if (bas.message.Contains(((int)errorTransfer.no_wagons).ToString())) return 0;
                    if (bas.message.Contains(((int)errorTransfer.no_wagon_is_list).ToString())) return 0;
                    if (bas.message.Contains(((int)errorTransfer.no_wagon_is_nathist).ToString())) return 0;
                    // Даем срок закрыть данные
                    if (bas.datetime < DateTime.Now.AddDays(-1 * this.day_range_arrival_kis_copy))
                    {
                        bas.close = DateTime.Now;
                        bas.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                        return ef_tkis.SaveRWBufferArrivalSostav(bas);
                    }
                }
            }
            // пропускаем
            return 0;
        }
        #endregion

        #region Коррекция системы переноса
        public int DeleteSostavRWBufferArrivalSostav(int id)
        {
            //EFTKIS ef_tkis = new EFTKIS();
            //EFRailCars ef_rc = new EFRailCars();
            //EFMetallurgTrans ef_mt = new EFMetallurgTrans();

            //TODO: Коррекция системы переноса - реализовать удаление из RAILWAY
            int del_rc = 0;
            //int err_del_rc = 0;
            //int del_sap = 0;
            //int upd_mt = 0;
            //try
            //{
            //    RWBufferArrivalSostav del_bas = ef_tkis.GetRWBufferArrivalSostav(id);
            //    RWBufferArrivalSostav new_bas = null;
            //    List<RWBufferArrivalSostav> not_close_list = ef_tkis.GetRWBufferArrivalSostav().Where(b => b.datetime >= del_bas.datetime & b.id != del_bas.id).ToList();
            //    foreach (RWBufferArrivalSostav bas in not_close_list)
            //    {
            //        if (bas.list_wagons != null && del_bas.list_wagons != null && bas.list_wagons.Trim() == del_bas.list_wagons.Trim())
            //        {
            //            new_bas = bas;
            //            break;
            //        }
            //    }
            //    string mess = String.Format("Коррекция данных (Удалена натурка {0} от {1}, создана новая {2} от {3}). ",
            //        del_bas.natur, del_bas.datetime, (new_bas != null ? (int?)new_bas.natur : null), (new_bas != null ? (DateTime?)new_bas.datetime : null));

            //    List<VAGON_OPERATIONS> list_del_wagons = ef_rc.GetVagonsOperationsToNatur(del_bas.natur, del_bas.datetime).ToList();
            //    foreach (VAGON_OPERATIONS wag in list_del_wagons)
            //    {
            //        int? way = wag.id_way;
            //        int idsostav = (int)wag.IDSostav;
            //        VAGON_OPERATIONS res_del = ef_rc.DeleteVAGON_OPERATIONS(wag.id_oper);
            //        if (res_del != null)
            //        {
            //            del_rc++;
            //            //String.Format(mess + "Из системы RailCars - удален вагон {0}", wag.num_vagon).WriteEvents(servece_owner, eventID);
            //            if (way != null)
            //            {
            //                ef_rc.OffSetCars((int)way, 1);
            //            }
            //            if (wag.IDSostav < 0)
            //            {
            //                // idsostav отрицательный
            //                EFSAP ef_sap = new EFSAP();
            //                ef_sap.DeleteSAPIncSupplySostav(idsostav);
            //                del_sap++;
            //            }
            //            if (wag.IDSostav > 0)
            //            {
            //                // idsostav положительный
            //                if (new_bas != null)
            //                {
            //                    int new_natur = new_bas.natur;
            //                    DateTime date_amkr = new_bas.datetime;
            //                    ArrivalCars arr_car = ef_mt.GetArrivalCars((int)wag.IDSostav);
            //                    if (arr_car != null)
            //                    {
            //                        arr_car.NumDocArrival = new_natur;
            //                        arr_car.Arrival = date_amkr;
            //                        arr_car.UserName = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
            //                        ef_mt.SaveArrivalCars(arr_car);
            //                        upd_mt++;
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            err_del_rc++;
            //        }
            //    }
            //    if (err_del_rc == 0)
            //    {
            //        del_bas.close = DateTime.Now;
            //        del_bas.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
            //        del_bas.status = (int)statusSting.Delete;
            //        ef_tkis.SaveRWBufferArrivalSostav(del_bas);
            //    }
            //    String.Format(mess + "Из системы RailCars - удалено {0} вагонов, ошибок удаления {1}, из справочника САП вхю пост. удалено {2} строк, в прибытии МТ Скорректировано {3} строки."
            //        , del_rc, err_del_rc, del_sap, upd_mt).WriteEvents(err_del_rc > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID);
            //}
            //catch (Exception e)
            //{
            //    e.WriteErrorMethod(String.Format("DeleteSostavBufferArrivalSostav(id={0})", id), servece_owner, eventID);
            //    return (int)errorTransfer.global;
            //}
            return del_rc;
        }
        #endregion


    }
}
