﻿using EFRW.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW
{
    public static class RWHelpers
    {
        private static eventID eventID = eventID.RW_RWHelpers;

        public delegate bool IsFilterStatusOperation(CarOperations operation);

        /// <summary>
        /// Фильтр операция открыта
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static bool IsOpen(this CarOperations operation)
        {
            return operation.dt_inp != null && operation.dt_out == null ? true : false;
        }
        /// <summary>
        /// Фильтр операция закрыта
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static bool IsClose(this CarOperations operation)
        {
            return operation.dt_inp != null && operation.dt_out != null ? true : false;
        }
        /// <summary>
        /// Выбрать опрерации согласно фильтра
        /// </summary>
        /// <param name="list"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<CarOperations> IsStatusOperatio(this IEnumerable<CarOperations> list, IsFilterStatusOperation filter)
        {
            List<CarOperations> result = new List<CarOperations>();
            foreach (CarOperations oper in list)
            {
                if (filter(oper))
                {
                    result.Add(oper);
                }
            }
            return result;
        }
    }
}