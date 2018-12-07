﻿using EFReference.Entities;
using EFRW.Concrete;
using EFRW.Concrete.EFDirectory;
using EFRW.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW
{
    /// <summary>
    /// Класс справочной систмы RailWay
    /// </summary>
    public class RWDirectory
    {
        private eventID eventID = eventID.RW_RWDirectory;
        private service servece_owner;
        private EFDbContext db;
        private bool log_detali = false;
        private bool reference_kis = true; // Использовать справочники КИС


        public RWDirectory()
        {
            this.db = new EFDbContext();
            this.servece_owner = service.Null;
        }

        public RWDirectory(EFDbContext db)
        {
            this.db = db;
            this.servece_owner = service.Null;
        }

        public RWDirectory(service servece_owner)
        {
            this.servece_owner = servece_owner;
            this.db = new EFDbContext();
        }

        public RWDirectory(EFDbContext db, service servece_owner)
        {
            this.servece_owner = servece_owner;
            this.db = db;
        }

        #region Справочник "Стран"
        /// <summary>
        /// Вернуть страну по коду 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Directory_Country GetCountryOfCode(int code) {
            try
            {
                EFDirectoryCountry ef_country = new EFDirectoryCountry(this.db);
                return ef_country.Get().Where(c => c.code == code).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCountryOfCode(code={0})", code), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строку справочника "Стран" по коду, если нет создвать новую
        /// </summary>
        /// <param name="code"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        protected Directory_Country GetCountry(Countrys code, bool create)
        {
            try
            {
                if (code == null) return null;
                if (code.code <= 0) return null;
                EFDirectoryCountry ef_country = new EFDirectoryCountry(this.db);

                Directory_Country country = GetCountryOfCode(code.code);
                if (country == null & create)
                {
                    country = new Directory_Country()
                    {
                        id = 0,
                        country_ru = code.country,
                        country_en = code.country,
                        code = code.code,
                    };
                    ef_country.Add(country);
                    ef_country.Save();
                }
                return country;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCountry(code={0}, create={1})", code, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить id страны из справочника "Стран" по коду, если нет создвать новую
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected int GetIDCountry(Countrys code)
        {
            try
            {
                Directory_Country country = GetCountry(code, true);
                return country != null ? country.id : 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDCountry(code={0})", code), servece_owner, eventID);
                return 0;
            }
        }
        /// <summary>
        /// Получить id строки справочника "Стран" системы RailWay по коду СНГ и стран балтии.
        /// </summary>
        /// <param name="code_country_sng"></param>
        /// <returns></returns>
        public int GetIDCountryOfCodeSNG(int code_country_sng)
        {
            try
            {
                EFReference.Concrete.EFReference ef_reference = new EFReference.Concrete.EFReference();
                Countrys code = ef_reference.GetCountryOfCodeSNG(code_country_sng);
                return GetIDCountry(code);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDCountryOfCodeSNG(code_country_sng={0})", code_country_sng), servece_owner, eventID);
                return 0;
            }
        }
        /// <summary>
        /// Получить id строки справочника "Стран" системы RailWay по коду МеталлургТранса
        /// </summary>
        /// <param name="code_mt"></param>
        /// <returns></returns>
        public int GetIDCountryOfCodeMT(string code_mt)
        {
            try
            {
                int country = int.Parse(code_mt.Substring(0, 2));
                return GetIDCountryOfCodeSNG(country);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDCountryOfCodeMT(code_mt={0})", code_mt), servece_owner, eventID);
                return 0;
            }
        }
        /// <summary>
        /// Получить id строки справочника "Стран" системы RailWay по коду iso
        /// </summary>
        /// <param name="code_country_iso"></param>
        /// <returns></returns>
        public int GetIDCountryOfCodeISO(int code_country_iso)
        {
            try
            {
                EFReference.Concrete.EFReference ef_reference = new EFReference.Concrete.EFReference();
                Countrys code = ef_reference.GetCountryOfCode(code_country_iso);
                return GetIDCountry(code);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDCountryOfCodeISO(code_country_sng={0})", code_country_iso), servece_owner, eventID);
                return 0;
            }
        }

        #endregion

        #region Справочник "Грузов"
        /// <summary>
        /// Получить строку справочника "Грузов" по коду ЕТСНГ,
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public Directory_Cargo GetCargoOfCodeETSNG(int code_etsng)
        {
            try
            {
                EFDirectoryCargo ef_cargo = new EFDirectoryCargo(this.db);
                return ef_cargo.Get().Where(c => c.etsng == code_etsng).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCargoOfCodeETSNG(code_etsng={0})", code_etsng), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть строку справочника "Грузов" по коду ЕТСНГ, если нет создать
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public Directory_Cargo GetCargoOfCodeETSNG(int code_etsng, bool create)
        {
            try
            {
                EFDirectoryCargo ef_cargo = new EFDirectoryCargo(this.db);
                EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
                Directory_Cargo cargo = GetCargoOfCodeETSNG(code_etsng);
                if (cargo == null)
                {
                    EFReference.Entities.Cargo ref_cargo = reference.GetCargoOfCodeETSNG(code_etsng);
                    if (ref_cargo != null & create)
                    {
                        cargo = new Directory_Cargo()
                        {
                            name_ru = ref_cargo.name_etsng.Length > 200 ? ref_cargo.name_etsng.Remove(199).Trim() : ref_cargo.name_etsng.Trim(),
                            name_en = ref_cargo.name_etsng.Length > 200 ? ref_cargo.name_etsng.Remove(199).Trim() : ref_cargo.name_etsng.Trim(),
                            name_full_ru = ref_cargo.name_etsng.Length > 500 ? ref_cargo.name_etsng.Remove(499).Trim() : ref_cargo.name_etsng.Trim(),
                            name_full_en = ref_cargo.name_etsng.Length > 500 ? ref_cargo.name_etsng.Remove(499).Trim() : ref_cargo.name_etsng.Trim(),
                            etsng = code_etsng,
                            id_type = 0
                        };
                    }
                    else
                    {
                        cargo = new Directory_Cargo()
                        {
                            name_ru = "?",
                            name_en = "?",
                            name_full_ru = "?",
                            name_full_en = "?",
                            etsng = code_etsng,
                            id_type = 0
                        };
                    }
                    ef_cargo.Add(cargo);
                    ef_cargo.Save();
                }
                return cargo;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCargoOfCodeETSNG(code_etsng={0}, create={1})", code_etsng, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть id строки справочника "Грузов" по коду ЕТСНГ, если нет создать
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public int GetIDCargoOfCodeETSNG(int code_etsng)
        {
            try
            {
                Directory_Cargo cargo = GetCargoOfCodeETSNG(code_etsng, true);
                return cargo != null ? cargo.id : 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDCargoOfCodeETSNG(code_etsng={0})", code_etsng), servece_owner, eventID);
                return 0;
            }
        }
        /// <summary>
        /// Вернуть id строки справочника "Грузов" по коду ЕТСНГ (с предварительной коррекцией), если нет создать
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public int GetIDCargoOfCorrectCodeETSNG(int code_etsng)
        {
            EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
            return GetIDCargoOfCodeETSNG(reference.GetCodeCorrectCargo(code_etsng));
        }

        //public int GetIDCorrectCodeETSNGOfKis(int? code_kis)
        //{
        //    if (code_kis != null)
        //    {
        //        EFKIS.Concrete.EFWagons kis = new EFKIS.Concrete.EFWagons();
        //        EFKIS.Entities.PromGruzSP pg = kis.GetGruzSP((int)code_kis);
        //        if (pg != null && pg.TAR_GR != null)
        //        {
        //            EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
        //            return reference.GetCodeCorrectCargo((int)pg.TAR_GR);
        //        }
        //    }
        //    return 0;
        //}

        #endregion

        #region Справочник "Станций railway"
        /// <summary>
        /// Получить строку справочника "Станций railway" по коду станции УЗ
        /// </summary>
        /// <param name="code_uz"></param>
        /// <returns></returns>
        public Directory_InternalStations GetInternalStationsOfCodeUZ(int code_uz)
        {
            try
            {
                EFDirectoryInternalStations ef_station = new EFDirectoryInternalStations(this.db);
                return ef_station.Get().Where(c => c.code_uz == code_uz).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInternalStationsOfCodeUZ(code_uz={0})", code_uz), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строку справочника "Станций railway" по коду станции из системы КИС
        /// </summary>
        /// <param name="id_kis"></param>
        /// <returns></returns>
        public Directory_InternalStations GetInternalStationsOfKIS(int id_kis)
        {
            try
            {
                EFDirectoryInternalStations ef_station = new EFDirectoryInternalStations(this.db);
                return ef_station.Get().Where(c => c.id_kis == id_kis).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInternalStationsOfKIS(id_kis={0})", id_kis), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть строку станции УЗ справочника "Станций railway" по индексу поезда МТ
        /// </summary>
        /// <param name="index_mt"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public Directory_InternalStations GetInternalStationsUZ(string index_mt, bool create)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(index_mt)) return null;
                EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
                int code = int.Parse(index_mt.Substring(9, 4));
                EFReference.Entities.Stations station_in = reference.GetStationsOfCode(code * 10);
                int codecs = station_in != null ? (int)station_in.code_cs : code * 10;
                return GetInternalStationsUZ(codecs, true);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInternalStationsUZ(index_mt={0}, create={1})", index_mt, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть строку станции УЗ справочника "Станций railway" по коду уз
        /// </summary>
        /// <param name="code_uz"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public Directory_InternalStations GetInternalStationsUZ(int code_uz, bool create)
        {
            try
            {
                EFDirectoryInternalStations ef_station = new EFDirectoryInternalStations(this.db);
                
                EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
                Directory_InternalStations station = GetInternalStationsOfCodeUZ(code_uz);
                // Если нет станции создадим
                if (station == null & create)
                {
                    Stations station_uz = reference.GetStationsOfCodeCS(code_uz);
                    station = new Directory_InternalStations()
                    {
                        id = 0,
                        name_ru = station_uz.station,
                        name_en = station_uz.station,
                        view = false,
                        exit_uz = false,
                        station_uz = true,
                        id_kis = null,
                        default_side = null,
                        code_uz = code_uz, 
                    };
                    ef_station.Add(station);
                    ef_station.Save();
                    // Если станция создались новая, добавим к ней пути 
                    if (station.id > 0) {
                        CreateDefaultWayStationUZ(station.id);
                    }
                }
                return station;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInternalStationsUZ(code_uz={0}, create={1})", code_uz, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть строку станции справочника "Станций railway" по id kis, если нет создать по данным КИС
        /// </summary>
        /// <param name="id_kis"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public Directory_InternalStations GetInternalStationsOfKis(int id_kis, bool create)
        {
            try
            {
                EFDirectoryInternalStations ef_station = new EFDirectoryInternalStations(this.db);
                EFKIS.Concrete.EFWagons kis = new EFKIS.Concrete.EFWagons();
                Directory_InternalStations station = GetInternalStationsOfKIS(id_kis);
                // Если нет станции создадим
                if (station == null & create)
                {
                    EFKIS.Entities.KometaStan st = kis.GetKometaStan(id_kis);
                    station = new Directory_InternalStations()
                    {
                        id = 0,
                        name_ru = st != null ? st.NAME : "?",
                        name_en = st != null ? st.NAME : "?",
                        view = false,
                        exit_uz = false,
                        station_uz = st != null && st.MPS != null ? (bool)st.MPS : false,
                        id_kis = null,
                        default_side = null,
                        code_uz = st != null && st.MPS != null ? (int?)0 : null,
                    };
                    ef_station.Add(station);
                    ef_station.Save();

                    // Если станция создались станция УЗ, добавим к ней пути 
                    if (station.id > 0 && station.station_uz)
                    {
                        CreateDefaultWayStationUZ(station.id);
                    }
                }
                return station;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInternalStationsOfKis(id_kis={0}, create={1})", id_kis, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть id станции справочника "Станций railway" по id kis, если нет создать по данным КИС 
        /// </summary>
        /// <param name="id_kis"></param>
        /// <returns></returns>
        public int GetIDInternalStationsOfKIS(int? id_kis)
        {
            try
            {
                if (id_kis == null) return 0;
                Directory_InternalStations station = GetInternalStationsOfKis((int)id_kis, this.reference_kis == true ? true : false);
                return station != null ? station.id : 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInternalStationsOfKis(id_kis={0})", id_kis), servece_owner, eventID);
                return 0;
            }
        }

        #endregion

        #region Справочник "Путей railway"
        /// <summary>
        /// Получить строку справочника "Путей railway" по коду станции и номеру пити
        /// </summary>
        /// <param name="id_station"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public Directory_Ways GetWaysOfStation(int id_station, string num)
        {
            try
            {
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                return ef_way.Get().Where(w => w.id_station == id_station & w.num == num).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWaysOfStation(id_station={0}, num={1})", id_station, num), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список строк справочника "Путей railway" по коду станции
        /// </summary>
        /// <param name="id_station"></param>
        /// <returns></returns>
        public IEnumerable<Directory_Ways> GetWaysOfStation(int id_station)
        {
            try
            {
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                return ef_way.Get().Where(w => w.id_station == id_station);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWaysOfStation(id_station={0})", id_station), eventID);
                return null;
            }
        }
        /// <summary>
        /// Создать на станции УЗ пути по умолчанию
        /// </summary>
        /// <param name="id_station"></param>
        /// <returns></returns>
        public bool CreateDefaultWayStationUZ(int id_station)
        {
            try
            {
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                Directory_Ways way_send_amkr = new Directory_Ways()
                {
                    id_station = id_station,
                    num = "1",
                    name_ru = "Путь для отправки на АМКР",
                    name_en = "The way to send to AMKR",
                    position = 1,
                    capacity = null,
                    id_type_way = 1,
                    id_car_status = 15,   // ожидаем прибытия с УЗ
                    id_station_end = null,
                    id_shop_end = null,
                    id_overturn_end = null,
                };
                ef_way.Add(way_send_amkr);
                Directory_Ways way_maneuver = new Directory_Ways()
                {
                    id_station = id_station,
                    num = "1",
                    name_ru = "Путь для маневра",
                    name_en = "The way to maneuver",
                    position = 2,
                    capacity = null,
                    id_type_way = 1,
                    id_car_status = 19, // ТСП по УЗ
                    id_station_end = null,
                    id_shop_end = null,
                    id_overturn_end = null,
                };
                ef_way.Add(way_maneuver);
                ef_way.Save();
                return way_send_amkr.id > 0 && way_maneuver.id > 0 ? true : false;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateDefaultWayStationUZ(id_station={0})", id_station), servece_owner, eventID);
                return false;
            }
        }
        /// <summary>
        /// Вернуть строку пути справочника "Пути railway" по id станции и номеру пути, если нет создать по данным КИС
        /// </summary>
        /// <param name="id_station"></param>
        /// <param name="num"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public Directory_Ways GetWay(int id_station, string num, bool create)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(num)) return null;
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                Directory_Ways way = GetWaysOfStation(id_station, num);
                // Если нет станции создадим
                if (way == null & create)
                {
                    // Создадим путь для отправки на АМКР
                    way = new Directory_Ways()
                    {
                        id_station = id_station,
                        num = num,
                        name_ru = "Путь создан по данным КИС",
                        name_en = "The path is based on the KIS data",
                        position = 0,
                        capacity = null,
                        id_type_way = 0,
                        id_car_status = null, 
                        id_station_end = null,
                        id_shop_end = null,
                        id_overturn_end = null,
                    };
                    ef_way.Add(way);
                    ef_way.Save();
                }
                return way;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWay(id_station={0}, num={1}, create={2})", id_station, num, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть id пути справочника "Пути railway" по id станции и номеру пути, если нет создать по данным КИС
        /// </summary>
        /// <param name="id_station"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public int GetIDWayOfStation(int id_station, string num)
        {
            try
            {
                Directory_Ways way = GetWay(id_station, num, this.reference_kis == true ? true : false);
                return way != null ? way.id : 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDWayOfStation(id_station={0}, num={1})", id_station, num), servece_owner, eventID);
                return 0;
            }
        }
        /// <summary>
        /// Вернуть id пути справочника "Пути railway" по id станции и номеру пути, если нет создать по данным КИС, если не создан вернуть первый путь по умолчанию
        /// </summary>
        /// <param name="id_station"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public int GetIDDefaultWayOfStation(int id_station, string num)
        {
            try
            {

                int id = GetIDWayOfStation(id_station, num);
                if (id == 0)
                {
                    Directory_Ways Way = GetWaysOfStation(id_station).OrderBy(w => w.num).FirstOrDefault();
                    if (Way != null) { id = Way.id; }
                }
                return id;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDDefaultWayOfStation(id_station={0}, num={1})", id_station, num), servece_owner, eventID);
                return 0;
            }
        }

        #endregion






    }
}
