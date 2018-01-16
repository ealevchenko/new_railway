﻿using MessageLog;
using RWSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KIS
{
    public class KISThread
    {
        private static eventID eventID = eventID.KIS_KISThread;
        protected static service servece_owner = service.Null;

        protected static object locker_setting = new object();
        protected static object locker_tas = new object();
        protected static object locker_bis = new object(); // блокировка доступа к таблице bufferInputSostav
        protected static object locker_bos = new object(); // блокировка доступа к таблице bufferOutputSostav

        protected Thread thCopyBufferArrivalSostav = null;
        public bool statusCopyBufferArrivalSostav { get { return thCopyBufferArrivalSostav.IsAlive; } }

        protected Thread thTransferArrivalOfKIS = null;
        public bool statusTransferArrivalOfKIS { get { return thTransferArrivalOfKIS.IsAlive; } }

        protected Thread thCloseBufferArrivalSostav = null;
        public bool statusCloseBufferArrivalSostav { get { return thCloseBufferArrivalSostav.IsAlive; } }

        protected Thread thCopyBufferInputSostav = null;
        public bool statusCopyBufferInputSostav { get { return thCopyBufferInputSostav.IsAlive; } }

        protected Thread thTransferInputKIS = null;
        public bool statusTransferInputKIS { get { return thTransferInputKIS.IsAlive; } }

        protected Thread thCopyBufferOutputSostav = null;
        public bool statusCopyBufferOutputSostav { get { return thCopyBufferOutputSostav.IsAlive; } }

        protected Thread thTransferOutputKIS = null;
        public bool statusTransferOutputKIS { get { return thTransferOutputKIS.IsAlive; } }

        public KISThread()
        {

        }

        public KISThread(service servece_name)
        {
            servece_owner = servece_name;
        }

        #region TransferArrival (Копирование по прибытию из станций УЗ Кривого Рога)

        #region CopyBufferArrivalSostav
        /// <summary>
        /// Запустить поток переноса информации о составах принятых в системе КИС
        /// </summary>
        /// <returns></returns>
        public bool StartCopyBufferArrivalSostav()
        {
            service service = service.CopyArrivalSostavKIS;
            string mes_service_start = String.Format("Поток : {0} сервиса : {1}", service.ToString(), servece_owner);
            try
            {
                if ((thCopyBufferArrivalSostav == null) || (!thCopyBufferArrivalSostav.IsAlive && thCopyBufferArrivalSostav.ThreadState == ThreadState.Stopped))
                {
                    thCopyBufferArrivalSostav = new Thread(CopyBufferArrivalSostav);
                    thCopyBufferArrivalSostav.Name = service.ToString();
                    thCopyBufferArrivalSostav.Start();
                }
                return thCopyBufferArrivalSostav.IsAlive;
            }
            catch (Exception ex)
            {
                mes_service_start += " - ошибка запуска.";
                ex.WriteError(mes_service_start, servece_owner, eventID);
                return false;
            }

        }
        /// <summary>
        /// Поток переноса информации о составах принятых в системе КИС
        /// </summary>
        private static void CopyBufferArrivalSostav()
        {
            service service = service.CopyArrivalSostavKIS;
            DateTime dt_start = DateTime.Now;
            try
            {
                int day_control_arrival_kis_add_data = 1;
                // считать настройки
                lock (locker_setting)
                {
                    try
                    {
                        // Период контроля добавления натурных листов прибытия
                        day_control_arrival_kis_add_data = RWSetting.GetDB_Config_DefaultSetting<int>("AddControlPeriodCopyArrivalSostav", service, day_control_arrival_kis_add_data, true);
                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения считывания настроек потока {0}, сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                    }
                }
                dt_start = DateTime.Now;
                int res_copy = 0;
                lock (locker_tas)
                {
                    // Проверить наличие новых прибытий в КИС, перенести данные в таблицу
                    KISTransfer kis_trans = new KISTransfer(service);
                    kis_trans.DayControlArrivalKisAddData = day_control_arrival_kis_add_data;
                    res_copy = kis_trans.CopyBufferArrivalSostavOfKIS();
                }
                TimeSpan ts = DateTime.Now - dt_start;
                string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2}:{3}:{4}({5}), код выполнения: res_copy:{6}", service.ToString(), servece_owner, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, res_copy);
                mes_service_exec.WriteInformation(servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, res_copy);
            }
            catch (ThreadAbortException exc)
            {
                String.Format("Поток {0} сервиса {1} - прерван по событию ThreadAbortException={2}", service.ToString(), servece_owner, exc).WriteWarning(servece_owner, eventID);
            }
            catch (Exception ex)
            {
                ex.WriteError(String.Format("Ошибка выполнения потока {0} сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, -1);

            }
        }
        #endregion

        #region TransferArrivalOfKIS
        /// <summary>
        /// Запустить поток переноса вагонов состава принятых в системе КИС
        /// </summary>
        /// <returns></returns>
        public bool StartTransferArrivalOfKIS()
        {
            service service = service.TransferArrivalKIS;
            string mes_service_start = String.Format("Поток : {0} сервиса : {1}", service.ToString(), servece_owner);
            try
            {
                if ((thTransferArrivalOfKIS == null) || (!thTransferArrivalOfKIS.IsAlive && thTransferArrivalOfKIS.ThreadState == ThreadState.Stopped))
                {
                    thTransferArrivalOfKIS = new Thread(TransferArrivalOfKIS);
                    thTransferArrivalOfKIS.Name = service.ToString();
                    thTransferArrivalOfKIS.Start();
                }
                return thTransferArrivalOfKIS.IsAlive;
            }
            catch (Exception ex)
            {
                mes_service_start += " - ошибка запуска.";
                ex.WriteError(mes_service_start, servece_owner, eventID);
                return false;
            }

        }
        /// <summary>
        /// Поток переноса вагонов состава принятых в системе КИС
        /// </summary>
        private static void TransferArrivalOfKIS()
        {
            service service = service.TransferArrivalKIS;
            DateTime dt_start = DateTime.Now;
            try
            {
                //int add_control_period = 1;
                // считать настройки
                lock (locker_setting)
                {
                    try
                    {
                        // Период контроля добавления натурных листов прибытия
                        //add_control_period = RWSetting.GetDB_Config_DefaultSetting<int>("AddControlPeriodCopyArrivalSostav", service, add_control_period, true);
                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения считывания настроек потока {0}, сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                    }
                }
                dt_start = DateTime.Now;
                int res_transfer = 0;
                lock (locker_tas)
                {
                    // Проверить наличие новых прибытий в КИС, перенести данные в таблицу
                    KISTransfer kis_trans = new KISTransfer(service);
                    res_transfer = kis_trans.TransferArrivalOfKIS();
                }
                TimeSpan ts = DateTime.Now - dt_start;
                string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2}:{3}:{4}({5}), код выполнения: res_transfer:{6}", service.ToString(), servece_owner, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, res_transfer);
                mes_service_exec.WriteInformation(servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, res_transfer);
            }
            catch (ThreadAbortException exc)
            {
                String.Format("Поток {0} сервиса {1} - прерван по событию ThreadAbortException={2}", service.ToString(), servece_owner, exc).WriteWarning(servece_owner, eventID);
            }
            catch (Exception ex)
            {
                ex.WriteError(String.Format("Ошибка выполнения потока {0} сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, -1);

            }
        }
        #endregion

        #region CloseBufferArrivalSostav
        /// <summary>
        /// Запустить поток закрытия составов в буфере переноса из КИС
        /// </summary>
        /// <returns></returns>
        public bool StartCloseBufferArrivalSostav()
        {
            service service = service.CloseArrivalSostavKIS;
            string mes_service_start = String.Format("Поток : {0} сервиса : {1}", service.ToString(), servece_owner);
            try
            {
                if ((thCloseBufferArrivalSostav == null) || (!thCloseBufferArrivalSostav.IsAlive && thCloseBufferArrivalSostav.ThreadState == ThreadState.Stopped))
                {
                    thCloseBufferArrivalSostav = new Thread(CloseBufferArrivalSostav);
                    thCloseBufferArrivalSostav.Name = service.ToString();
                    thCloseBufferArrivalSostav.Start();
                }
                return thCloseBufferArrivalSostav.IsAlive;
            }
            catch (Exception ex)
            {
                mes_service_start += " - ошибка запуска.";
                ex.WriteError(mes_service_start, servece_owner, eventID);
                return false;
            }

        }
        /// <summary>
        /// Поток закрытия составов в буфере переноса из КИС
        /// </summary>
        private static void CloseBufferArrivalSostav()
        {
            service service = service.CloseArrivalSostavKIS;
            DateTime dt_start = DateTime.Now;
            try
            {
                int day_range_arrival_kis_copy = 2; // тайм аут (суток) по времени для составов перенесеных из КИС для копирования в систему RailCars
                // считать настройки
                lock (locker_setting)
                {
                    try
                    {
                        // Период контроля добавления натурных листов прибытия
                        day_range_arrival_kis_copy = RWSetting.GetDB_Config_DefaultSetting<int>("DayRangeArrivalKisCopy", service, day_range_arrival_kis_copy, true);
                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения считывания настроек потока {0}, сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                    }
                }
                dt_start = DateTime.Now;
                int res_close = 0;
                lock (locker_tas)
                {
                    // Проверить наличие новых прибытий в КИС, перенести данные в таблицу
                    KISTransfer kis_trans = new KISTransfer(service);
                    kis_trans.DayRangeArrivalKisCopy = day_range_arrival_kis_copy; //тайм аут (суток) по времени для составов перенесеных из КИС для копирования в систему RailCars
                    res_close = kis_trans.CloseBufferArrivalSostav();
                }
                TimeSpan ts = DateTime.Now - dt_start;
                string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2}:{3}:{4}({5}), код выполнения: res_close:{6}", service.ToString(), servece_owner, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, res_close);
                mes_service_exec.WriteInformation(servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, res_close);
            }
            catch (ThreadAbortException exc)
            {
                String.Format("Поток {0} сервиса {1} - прерван по событию ThreadAbortException={2}", service.ToString(), servece_owner, exc).WriteWarning(servece_owner, eventID);
            }
            catch (Exception ex)
            {
                ex.WriteError(String.Format("Ошибка выполнения потока {0} сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, -1);

            }
        }

        #endregion        
        #endregion

        #region TransferInput (Копирование по прибытию из станций АМКР)

        #region CopyBufferInputSostav
        /// <summary>
        /// Запустить поток переноса информации о составах принятых в системе КИС
        /// </summary>
        /// <returns></returns>
        public bool StartCopyBufferInputSostav()
        {
            service service = service.CopyInputSostavKIS;
            string mes_service_start = String.Format("Поток : {0} сервиса : {1}", service.ToString(), servece_owner);
            try
            {
                if ((thCopyBufferInputSostav == null) || (!thCopyBufferInputSostav.IsAlive && thCopyBufferInputSostav.ThreadState == ThreadState.Stopped))
                {
                    thCopyBufferInputSostav = new Thread(CopyBufferInputSostav);
                    thCopyBufferInputSostav.Name = service.ToString();
                    thCopyBufferInputSostav.Start();
                }
                return thCopyBufferInputSostav.IsAlive;
            }
            catch (Exception ex)
            {
                mes_service_start += " - ошибка запуска.";
                ex.WriteError(mes_service_start, servece_owner, eventID);
                return false;
            }

        }
        /// <summary>
        /// Поток переноса информации о составах принятых в системе КИС
        /// </summary>
        private static void CopyBufferInputSostav()
        {
            service service = service.CopyInputSostavKIS;
            DateTime dt_start = DateTime.Now;
            try
            {
                int day_control_input_kis_add_data = 1;
                // считать настройки
                lock (locker_setting)
                {
                    try
                    {
                        // Период контроля добавления новых строк
                        day_control_input_kis_add_data = RWSetting.GetDB_Config_DefaultSetting<int>("DayControlInputKisAddData", service, day_control_input_kis_add_data, true);
                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения считывания настроек потока {0}, сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                    }
                }
                dt_start = DateTime.Now;
                int res_copy = 0;
                lock (locker_bis)
                {
                    // Проверить наличие новых прибытий в КИС, перенести данные в таблицу
                    KISTransfer kis_trans = new KISTransfer(service);
                    kis_trans.DayControlInputKisAddData = day_control_input_kis_add_data;
                    res_copy = kis_trans.CopyBufferInputSostavOfKIS();
                }
                TimeSpan ts = DateTime.Now - dt_start;
                string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2}:{3}:{4}({5}), код выполнения: res_copy:{6}", service.ToString(), servece_owner, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, res_copy);
                mes_service_exec.WriteInformation(servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, res_copy);
            }
            catch (ThreadAbortException exc)
            {
                String.Format("Поток {0} сервиса {1} - прерван по событию ThreadAbortException={2}", service.ToString(), servece_owner, exc).WriteWarning(servece_owner, eventID);
            }
            catch (Exception ex)
            {
                ex.WriteError(String.Format("Ошибка выполнения потока {0} сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, -1);

            }
        }
        #endregion

        #region TransferInputKIS
        /// <summary>
        /// Запустить поток переноса вагонов состава принятых в системе КИС
        /// </summary>
        /// <returns></returns>
        public bool StartTransferInputKIS()
        {
            service service = service.TransferInputKIS;
            string mes_service_start = String.Format("Поток : {0} сервиса : {1}", service.ToString(), servece_owner);
            try
            {
                if ((thTransferInputKIS == null) || (!thTransferInputKIS.IsAlive && thTransferInputKIS.ThreadState == ThreadState.Stopped))
                {
                    thTransferInputKIS = new Thread(TransferInputKIS);
                    thTransferInputKIS.Name = service.ToString();
                    thTransferInputKIS.Start();
                }
                return thTransferInputKIS.IsAlive;
            }
            catch (Exception ex)
            {
                mes_service_start += " - ошибка запуска.";
                ex.WriteError(mes_service_start, servece_owner, eventID);
                return false;
            }

        }
        /// <summary>
        /// Поток переноса вагонов состава принятых в системе КИС
        /// </summary>
        private static void TransferInputKIS()
        {
            service service = service.TransferInputKIS;
            DateTime dt_start = DateTime.Now;
            try
            {
                bool transfer_kis = false;
                // считать настройки
                lock (locker_setting)
                {
                    try
                    {
                        // Бит перосить данные из кис или просто закрыть строку
                        transfer_kis = RWSetting.GetDB_Config_DefaultSetting<bool>("TransferInputKIS", service, transfer_kis, true);
                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения считывания настроек потока {0}, сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                    }
                }
                dt_start = DateTime.Now;
                int res_transfer = 0;
                lock (locker_bis)
                {
                    // Проверить наличие новых прибытий в КИС, перенести данные в таблицу
                    KISTransfer kis_trans = new KISTransfer(service);
                    kis_trans.TransferInputKis = transfer_kis;
                    res_transfer = kis_trans.TransferArrivalOfKISInput();
                }
                TimeSpan ts = DateTime.Now - dt_start;
                string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2}:{3}:{4}({5}), код выполнения: res_transfer:{6}", service.ToString(), servece_owner, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, res_transfer);
                mes_service_exec.WriteInformation(servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, res_transfer);
            }
            catch (ThreadAbortException exc)
            {
                String.Format("Поток {0} сервиса {1} - прерван по событию ThreadAbortException={2}", service.ToString(), servece_owner, exc).WriteWarning(servece_owner, eventID);
            }
            catch (Exception ex)
            {
                ex.WriteError(String.Format("Ошибка выполнения потока {0} сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, -1);

            }
        }
        #endregion

        #endregion

        #region TransferOutput (Копирование по отправке на станцию АМКР)
        #region CopyBufferOutputSostav
        /// <summary>
        /// Запустить поток переноса информации о составах принятых в системе КИС
        /// </summary>
        /// <returns></returns>
        public bool StartCopyBufferOutputSostav()
        {
            service service = service.CopyOutputSostavKIS;
            string mes_service_start = String.Format("Поток : {0} сервиса : {1}", service.ToString(), servece_owner);
            try
            {
                if ((thCopyBufferOutputSostav == null) || (!thCopyBufferOutputSostav.IsAlive && thCopyBufferOutputSostav.ThreadState == ThreadState.Stopped))
                {
                    thCopyBufferOutputSostav = new Thread(CopyBufferOutputSostav);
                    thCopyBufferOutputSostav.Name = service.ToString();
                    thCopyBufferOutputSostav.Start();
                }
                return thCopyBufferOutputSostav.IsAlive;
            }
            catch (Exception ex)
            {
                mes_service_start += " - ошибка запуска.";
                ex.WriteError(mes_service_start, servece_owner, eventID);
                return false;
            }

        }
        /// <summary>
        /// Поток переноса информации о составах принятых в системе КИС
        /// </summary>
        private static void CopyBufferOutputSostav()
        {
            service service = service.CopyOutputSostavKIS;
            DateTime dt_start = DateTime.Now;
            try
            {
                int day_control_output_kis_add_data = 1;
                bool status_control_output_kis = false; // Контроль состояния закрытия строки системы КИС вагоны по отправке.
                // считать настройки
                lock (locker_setting)
                {
                    try
                    {
                        // Период контроля добавления новых строк
                        day_control_output_kis_add_data = RWSetting.GetDB_Config_DefaultSetting<int>("DayControlOutputKisAddData", service, day_control_output_kis_add_data, true);
                        // Контроль состояния закрытия строки системы КИС вагоны по отправке.
                        status_control_output_kis = RWSetting.GetDB_Config_DefaultSetting<bool>("StatusControlOutputKis", service, status_control_output_kis, true);
                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения считывания настроек потока {0}, сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                    }
                }
                dt_start = DateTime.Now;
                int res_copy = 0;
                lock (locker_bos)
                {
                    // Проверить наличие новых прибытий в КИС, перенести данные в таблицу
                    KISTransfer kis_trans = new KISTransfer(service);
                    kis_trans.DayControlOutputKisAddData = day_control_output_kis_add_data;
                    kis_trans.StatusControlOutputKis = status_control_output_kis;
                    res_copy = kis_trans.CopyBufferOutputSostavOfKIS();
                }
                TimeSpan ts = DateTime.Now - dt_start;
                string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2}:{3}:{4}({5}), код выполнения: res_copy:{6}", service.ToString(), servece_owner, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, res_copy);
                mes_service_exec.WriteInformation(servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, res_copy);
            }
            catch (ThreadAbortException exc)
            {
                String.Format("Поток {0} сервиса {1} - прерван по событию ThreadAbortException={2}", service.ToString(), servece_owner, exc).WriteWarning(servece_owner, eventID);
            }
            catch (Exception ex)
            {
                ex.WriteError(String.Format("Ошибка выполнения потока {0} сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, -1);

            }
        }
        #endregion

        #region TransferOutputKIS
        /// <summary>
        /// Запустить поток переноса вагонов состава принятых в системе КИС
        /// </summary>
        /// <returns></returns>
        public bool StartTransferOutputKIS()
        {
            service service = service.TransferOutputKIS;
            string mes_service_start = String.Format("Поток : {0} сервиса : {1}", service.ToString(), servece_owner);
            try
            {
                if ((thTransferOutputKIS == null) || (!thTransferOutputKIS.IsAlive && thTransferOutputKIS.ThreadState == ThreadState.Stopped))
                {
                    thTransferOutputKIS = new Thread(TransferOutputKIS);
                    thTransferOutputKIS.Name = service.ToString();
                    thTransferOutputKIS.Start();
                }
                return thTransferOutputKIS.IsAlive;
            }
            catch (Exception ex)
            {
                mes_service_start += " - ошибка запуска.";
                ex.WriteError(mes_service_start, servece_owner, eventID);
                return false;
            }

        }
        /// <summary>
        /// Поток переноса вагонов состава принятых в системе КИС
        /// </summary>
        private static void TransferOutputKIS()
        {
            service service = service.TransferOutputKIS;
            DateTime dt_start = DateTime.Now;
            try
            {
                bool transfer_kis = false;
                // считать настройки
                lock (locker_setting)
                {
                    try
                    {
                        // Бит перосить данные из кис или просто закрыть строку
                        transfer_kis = RWSetting.GetDB_Config_DefaultSetting<bool>("TransferOutputKIS", service, transfer_kis, true);
                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения считывания настроек потока {0}, сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                    }
                }
                dt_start = DateTime.Now;
                int res_transfer = 0;
                lock (locker_bos)
                {
                    // Проверить наличие новых прибытий в КИС, перенести данные в таблицу
                    KISTransfer kis_trans = new KISTransfer(service);
                    kis_trans.TransferOutputKis = transfer_kis;
                    res_transfer = kis_trans.TransferArrivalOfKISOutput();
                }
                TimeSpan ts = DateTime.Now - dt_start;
                string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2}:{3}:{4}({5}), код выполнения: res_transfer:{6}", service.ToString(), servece_owner, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, res_transfer);
                mes_service_exec.WriteInformation(servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, res_transfer);
            }
            catch (ThreadAbortException exc)
            {
                String.Format("Поток {0} сервиса {1} - прерван по событию ThreadAbortException={2}", service.ToString(), servece_owner, exc).WriteWarning(servece_owner, eventID);
            }
            catch (Exception ex)
            {
                ex.WriteError(String.Format("Ошибка выполнения потока {0} сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, -1);

            }
        }
        #endregion
        #endregion

    }
}
