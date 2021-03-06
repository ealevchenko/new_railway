﻿using EFReference.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFReference.Abstract
{
    public interface IReference
    {
        #region Cargo
        IQueryable<Cargo> Cargo { get; }
        IQueryable<Cargo> GetCargo();
        Cargo GetCargo(int id);
        int SaveCargo(Cargo Cargo);
        Cargo DeleteCargo(int id);
        Cargo GetCargoOfCodeETSNG(int code_etsng);
        IQueryable<Cargo> GetCargoOfCodeETSNG(int code_etsng_start, int code_etsng_stop);
        Cargo GetCorrectCargo(int code_etsng);
        int GetCodeCorrectCargo(int code_etsng);
        #endregion

        #region Stations
        IQueryable<Stations> Stations { get; }
        IQueryable<Stations> GetStations();
        Stations GetStations(int id);
        int SaveStations(Stations Stations);
        Stations DeleteStations(int id);
        Stations GetStationsOfCode(int code);
        IQueryable<Stations>  GetStationsOfCode(int code_start, int code_stop);
        Stations GetCorrectStationsOfCode(int code, bool check);
        Stations GetStationsOfCodeCS(int code_cs);
        #endregion

        #region Countrys
        IQueryable<Countrys> Countrys { get; }
        IQueryable<Countrys> GetCountrys();
        Countrys GetCountrys(int id);
        int SaveCountrys(Countrys Countrys);
        Countrys DeleteCountrys(int id);
        Countrys GetCountryOfCodeSNG(int code_sng);
        Countrys GetCountryOfCode(int code_iso);
        #endregion

        #region States
        IQueryable<States> States {get;}
        IQueryable<States> GetStates();
        States GetStates(int code);
        string GetStatesToState(int code);
        #endregion

        #region InternalRailroad (внутрение железные дороги)

        IQueryable<InternalRailroad> InternalRailroad {get;}
        IQueryable<InternalRailroad> GetInternalRailroad();
        InternalRailroad GetInternalRailroad(int id);

        #endregion

    }
}
