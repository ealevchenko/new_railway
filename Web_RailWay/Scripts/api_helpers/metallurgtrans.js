﻿//-------------------------------------------------------
// Для работы с модулем добавте файл common.js
//-------------------------------------------------------
// Веруть список всех вагонов за которыми установлено слижение (онлайн)
function getAsyncWagonsTracking(callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/wagons_tracking/',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}

// Веруть список составов на УЗ КР по которым есть не принятые вагоны
function getNoCloseArrivalCarsOfStationUZ(code, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/arrival/cars/station_uz/'+code,
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}

// Получить список вагонов по указанному прибытию
function getAsyncCarsOfArrivalUZ(id_sostav, is_close, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/arrival/cars/sostav/' + id_sostav + '/close/'+is_close.toString(),
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}


//******************************************************************************************
// Admins
//******************************************************************************************
// Получить список полученых xml-файлов
function getAsyncCountArrivalNaturList(callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/arrival/natur_list/',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
// Получить список полученых txt-файлов
function getAsyncCountApproachesNaturList(callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/approaches/natur_list/',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}

//******************************************************************************************
// WagonTracking
//******************************************************************************************
//------------------------------------------------------------------------------------------
// Вернуть список рапартов
function getAsyncWTReports(callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/wagons_tracking/reports',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
//------------------------------------------------------------------------------------------
// Вернуть последний маршрут по группе вагонов за указанное время
function getAsyncLastWagonTrackingOfReports(id_reports, start, stop, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/wagons_tracking_arhiv/last/report/' + id_reports + '/start/' + start.toISOString() + '/stop/' + stop.toISOString(),
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
// Вернуть последний маршрут по группе вагонов за указанное время
function getAsyncLastWagonTrackingAndDateTimeOfReports(id_reports, start, stop, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/wagons_tracking_arhiv/last_dt/report/' + id_reports + '/start/' + start.toISOString() + '/stop/' + stop.toISOString(),
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
//------------------------------------------------------------------------------------------
// Вернуть все маршруты по группе вагонов за указанное время
function getAsyncRouteWagonTrackingOfReports(id_reports, start, stop, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/wagons_tracking_arhiv/route/report/' + id_reports + '/start/' + start.toISOString() + '/stop/' + stop.toISOString(),
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
//Вернуть все маршруты по группе вагонов за указанное время
function getAsyncRouteWagonTrackingAndDateTimeOfReports(id_reports, start, stop, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/wagons_tracking_arhiv/route_dt/report/' + id_reports + '/start/' + start.toISOString() + '/stop/' + stop.toISOString(),
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
//------------------------------------------------------------------------------------------
// Вернуть текущие маршруты по группе вагонов за указанное время
function getAsyncLastRouteWagonTrackingOfReports(id_reports, start, stop, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/wagons_tracking_arhiv/route/last/report/' + id_reports + '/start/' + start.toISOString() + '/stop/' + stop.toISOString(),
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
// Вернуть текущие маршруты по группе вагонов за указанное время
function getAsyncLastRouteWagonTrackingAndDateTimeOfReports(id_reports, start, stop, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/wagons_tracking_arhiv/route_dt/last/report/' + id_reports + '/start/' + start.toISOString() + '/stop/' + stop.toISOString(),
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
//------------------------------------------------------------------------------------------
// Вернуть последние операции по группе вагонов
function getAsyncLastOperationWagonsTrackingOfCarsReports(id_reports, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/wagons_tracking_arhiv/operations/last/report/' + id_reports,
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
// Вернуть список операций по указанному вагону за указаный период времени
function getAsyncOperationWagonsTrackingOfNumCarAndDT(num, start, stop, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/wagons_tracking_arhiv/operations/car/' + num + '/start/' + start.toISOString() + '/stop/' + stop.toISOString(),
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
// Вернуть список операций по указаному вагону за указаный диапазон id
function getAsyncOperationWagonsTrackingOfNumCar(nvagon, id_start, id_stop, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/wagons_tracking_arhiv/operations/car/' + nvagon + '/id_start/' + id_start + '/id_stop/' + id_stop,
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
// Вернуть операции дислокации грузов
function getAsyncCargoDislocationOperationWagonsTrackingOfCarsReports(id_reports, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/wagons_tracking_arhiv/operations/cargo/disl/report/' + id_reports,
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}

//******************************************************************************************
// Arrival
//******************************************************************************************
// Вернуть список прибытий указаному вагону
function getAsyncArrivalCars(num, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/arrival/cars/num/'+num,
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}

//******************************************************************************************
// Списки 
//******************************************************************************************
// Вернуть список результатов закрытия прибытий
function getAsyncArrivalResult(result, callback) {
    $.ajax({
        type: 'GET',
        url: result!=null ? '/railway/api/mt/list/arrival/result/' + result : '/railway/api/mt/list/arrival/result',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}

//------------------------------------------------------------------------
// Формирование библиотек
//------------------------------------------------------------------------
function getReferenceArrivalResult(callback) {
    var ref = {
        list: [],
        initObject: function () {
            getAsyncArrivalResult(null, function (result) {
                ref.list = result;
                if (typeof callback === 'function') {
                    callback(ref);
                }
            });
        },
        getResult: function (value) {
            var result = getObjects(this.list, 'value', value)
            if (result != null && result.length > 0) {
                return result[0];
            }
        },
    }
    ref.initObject();
}


