﻿using EFKIS.Concrete;
using KIS;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    public class Test_KIS
    {
        public Test_KIS() { }

        
        
        public void Test_KISTransfer_TransferArrivalKISToRailWay()
        {
            KISTransfer kis_trans = new KISTransfer();
            kis_trans.CopyBufferArrivalSostavOfKIS(1);
        }
        /// <summary>
        /// Поставить на путь
        /// </summary>
        public void Test_KISTransfer_PutCarsToStation()
        {
            KISTransfer kis_trans = new KISTransfer();
            EFTKIS ef_tkis = new EFTKIS();
            EFKIS.Entities.BufferArrivalSostav arr_s = ef_tkis.GetBufferArrivalSostav(44);
            kis_trans.PutCarsToStation(ref arr_s);
        }

        public void Test_KISTransfer_UpdateCarsToStation()
        {
            KISTransfer kis_trans = new KISTransfer();
            EFTKIS ef_tkis = new EFTKIS();
            EFKIS.Entities.BufferArrivalSostav arr_s = ef_tkis.GetBufferArrivalSostav(44);
            kis_trans.UpdateCarsToStation(ref arr_s);
        }

        public void Test_KISThread_StartCopyBufferArrivalSostav()
        {
            KISThread kis_t = new KISThread(service.ServicesKIS);
            kis_t.StartCopyBufferArrivalSostav();
            kis_t.StartCopyBufferArrivalSostav();
        }

        public void Test_KISThread_StartTransferArrivalOfKIS()
        {
            KISThread kis_t = new KISThread(service.ServicesKIS);
            kis_t.StartTransferArrivalOfKIS();
        }
        /// <summary>
        /// тест коррекции (удалили натурный лист)
        /// </summary>
        public void Test_KISTransfer_DeleteSostavBufferArrivalSostav()
        {
            KISTransfer kis_trans = new KISTransfer();
            kis_trans.DeleteSostavBufferArrivalSostav(217);
        }

        public void Test_KISTransfer_CloseBufferArrivalSostav()
        {
            KISTransfer kis_trans = new KISTransfer();
            kis_trans.CloseBufferArrivalSostav();
        }

        public void Test_KISThread_StartCloseBufferArrivalSostav()
        {
            KISThread kis_t = new KISThread(service.ServicesKIS);
            kis_t.StartCloseBufferArrivalSostav();
        }


        /// <summary>
        /// Тест копирования по прибытию
        /// </summary>
        public void Test_KISTransfer_CopyBufferInputSostavOfKIS()
        {
            KISTransfer kis_trans = new KISTransfer(service.ServicesKIS);
            kis_trans.CopyBufferInputSostavOfKIS(1);
        }

        public void Test_KISThread_StartCopyBufferInputSostav()
        {
            KISThread kis_t = new KISThread(service.ServicesKIS);
            kis_t.StartCopyBufferInputSostav();
        }


        public void Test_KISTransfer_CopyBufferOutputSostavOfKIS()
        {
            KISTransfer kis_trans = new KISTransfer(service.ServicesKIS);
            kis_trans.CopyBufferOutputSostavOfKIS(1);
        }

        public void Test_KISThread_StartCopyBufferOutputSostav()
        {
            KISThread kis_t = new KISThread(service.ServicesKIS);
            kis_t.StartCopyBufferOutputSostav();
        }

    }
}
