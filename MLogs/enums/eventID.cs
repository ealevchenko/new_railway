﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLog
{
    public enum eventID : int
    {
        Null = -1,
        RailWay = 0,
        Test = 1,
        #region Вспомогательные общие модуля 1000

        RWSettings_RWSetting = 1100, // Библиотека доступа к настройкам сервисов БД, *.config файлы

        RWWebAPI = 1200,                // Библиотеки получения данных с WebApi RailWay
        RWWebAPI_ClientWebAPI = 1201,   // Библиотека клиент WebApi RailWay
        RWWebAPI_RWReference = 1202,    // Библиотека получения данных справочников ситемы RailWay
        RWWebAPI_Reference = 1203,      // Библиотека получения данных общих справочников железных дорог

        EFAccess = 1300,                // Библиотеки доступа к данным системы RailWay
        EFAccess_EFWebAcces = 1301,     // Библиотеки доступа к данным web-сайта RailWay

        WebApi = 1400,                      // Библиотеки доступа к WebApi через Token
        WebApi_ClientMetallurgTrans = 1401, // Библиотеки доступа к WebApi MT
        #endregion

        //RWSettings_RWDBSetting = 1110,    
    
        //Sockets_EFSockets = 1200,
        //Sockets_RWSocketBase = 1210,
        //Sockets_RWSocketServer = 1220,
        //Sockets_RWSocketClient = 1230,
        
        //// Новые сервисы работающие как отдельные потоки
        //RWService_MT_Transfer = 2000, // new
        //RWService_KIS_Transfer = 3000, // new
        //RWService_RailWay = 4000, // new

        #region Служба МеталлургТранс 2000
        MTTServices = 2000,                 // Служба
        
        EFMetallurgTrans = 2100,            // Библиотека доступа к данным БД MT

        MetallurgTrans = 2200,              // Библиотека сервисов обработки данных МеталлургТранс
        MetallurgTrans_MTTransfer = 2201,   // Модуль переноса данных МеталлургТранс
        MetallurgTrans_SFTPClient = 2202,   // Модуль доступа к SFTP серверам (данных МеталлургТранс)
        MetallurgTrans_MTThread = 2203,     // Модуль потоков переноса данных
        #endregion

        #region Служба RailCars 3000
        EFRailCars = 3100,                  // Библиотека доступа к данным БД системы RailCars
        RCReference = 3200,                 // Библиотека доступа к справочникам системы RailCars
        EFSAP = 3300,                       // Библиотека доступа к справочникам обновляемых SAP
        EFRCReference = 3400,               // Библиотека доступа к справочникам системы RailCars+
        RailCars = 3500,                    // Библиотека сервисов обработки данных Railcars

        #endregion

        #region Служба RailWay 4000

        EFRW = 4100,                                // Библиотека доступа к данным БД RailWay
        EFRW_EFReference = 4101,                    // Модуль доступа к БД справочников системы RailWay
        EFRW_EFRailWay = 4102,                      // Библиотека доступа к данным БД RailWay

        EFRW_EFDirectoryCars = 4110,                // Библиотека доступа к данным Directory_Cars БД RailWay
        EFRW_EFDirectoryTypeCars = 4111,            // Библиотека доступа к данным Directory_TypeCars БД RailWay
        EFRW_EFDirectoryGroupCars = 4112,           // Библиотека доступа к данным Directory_GroupCars БД RailWay
        EFRW_EFDirectoryCargo = 4113,               // Библиотека доступа к данным Directory_Cargo БД RailWay
        EFRW_EFDirectoryTypeCargo = 4114,           // Библиотека доступа к данным Directory_TypeCargo БД RailWay
        EFRW_EFDirectoryGroupCargo = 4115,          // Библиотека доступа к данным Directory_GroupCargo БД RailWay
        EFRW_EFDirectoryOwners = 4116,              // Библиотека доступа к данным Directory_Owners БД RailWay
        EFRW_EFDirectoryOwnerCars = 4117,           // Библиотека доступа к данным Directory_OwnerCars БД RailWay
        EFRW_EFDirectoryInternalStations = 4118,    // Библиотека доступа к данным Directory_InternalStations БД RailWay
        EFRW_EFDirectoryShops = 4119,               // Библиотека доступа к данным Directory_Shops БД RailWay
        EFRW_EFDirectoryOverturn = 4120,            // Библиотека доступа к данным Directory_Overturn БД RailWay
        EFRW_EFDirectoryWays = 4121,                // Библиотека доступа к данным Directory_Ways БД RailWay
        EFRW_EFDirectoryTypeWays = 4122,            // Библиотека доступа к данным Directory_TypeWays БД RailWay
        EFRW_EFDirectoryConsignee = 4123,           // Библиотека доступа к данным Directory_Consignee БД RailWay
        EFRW_EFDirectoryCountry = 4124,             // Библиотека доступа к данным Directory_Country БД RailWay
        EFRW_EFDirectoryExternalStations = 4125,    // Библиотека доступа к данным Directory_ExternalStations БД RailWay

        EFRW_EFCarsInternal = 4150,                 // Библиотека доступа к данным CarsInternal БД RailWay
        EFRW_EFCarOperations = 4151,                // Библиотека доступа к данным CarOperations БД RailWay
        EFRW_EFCarInboundDelivery = 4152,           // Библиотека доступа к данным CarInboundDelivery БД RailWay
        EFRW_EFCarOutboundDelivery = 4153,          // Библиотека доступа к данным CarOutboundDelivery БД RailWay
        EFRW_EFCarConditions = 4154,                // Библиотека доступа к данным CarConditions БД RailWay
        EFRW_EFCarStatus = 4155,                    // Библиотека доступа к данным CarStatus БД RailWay

        RW = 4200,                                  // Библиотека сервисов обработки данных RailWay
        RW_RWCars = 4201,                           // Модуль класса вагоны системы RailWay
        RW_RWDirectory = 4202,                      // Модуль класса справочники системы RailWay

        RW_RWFilters = 4210,                        // Модуль переноса данных системы RailWay
        RW_RWOperation = 4220,                      // Модуль операций системы RailWay (TODO: Этот модуль удалить)
        RW_RWOperations = 4221,                     // Модуль операций системы RailWay
        RW_RWReference = 4222,                      // Модуль спрвочных данных системы RailWay

        RW_RWHelpers = 4230,                        // Вспомогательный класс статических методов
        #endregion

        #region Служба общей справочной системы 5000
        EFReference = 5100,                 // Библиотека доступа к данным обшим справочникам ЖД

        #endregion

        #region Служба системы КИС 6000         
        KISTServices = 6000,                    // Служба
        EFWagons = 6100,                        // Библиотека доступа к данным БД системы КИС
        EFTKIS = 6200,                          // Библиотека доступа к таблицам переноса данных из системы КИС в систему RailCars, RailWay 
        KIS = 6300,                             // Библиотека сервисов обработки данных КИС
        KIS_KISTransfer = 6301,                 // Модуль переноса данных КИС
        KIS_RCTransfer = 6302,
        KIS_RWTransfer = 6303, 
        KIS_SAPTransfer = 6305,                 // Модуль переноса данных SAP
        KIS_KISThread = 6306,                   // Модуль потоков сервисов КИС
        #endregion

        #region Модуля согласования старой ситемы Railcars c новой Railway 10000
        OLDVersion = 10000,                 // Библиотеки старой системы RailCars
        OLDVersion_TRailCars = 10100,       // Библиотека переноса вагонов в старую систему RailCars
        #endregion

        Web_API_KisKometaController = 11101,
        Web_API_KisNumVagController = 11102,
        Web_API_KisPromController = 11103,

        Web_API_RCController = 11110,

        Web_API_MTController = 11120,

        Web_API_RWController = 11130,

        Web_API_ReferenceController = 11140,

    }
}
