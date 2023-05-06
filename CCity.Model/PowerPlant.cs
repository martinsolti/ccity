﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CCity.Model
{
    public class PowerPlant : Placeable, IFlammable, IMultifield
    {

        #region Fields

        List<Filler> _occupies;

        #endregion

        #region Properties

        public override int PlacementCost => 100;

        public override int MaintenanceCost => 10;

        double IFlammable.Pontential => throw new NotImplementedException();

        double IFlammable.Health => throw new NotImplementedException();

        bool IFlammable.IsOnFire { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        int IMultifield.Width => 2;

        int IMultifield.Height => 2;

        List<Filler> IMultifield.Occupies { get => _occupies; set => _occupies = value; }

        #endregion

        #region Constructor

        public PowerPlant()
        {
            _occupies = new();
        }

        public override void MakeRoot(SpreadType spreadType)
        {
            if (spreadType != SpreadType.Electricity) return;
            base.MakeRoot(spreadType);
            MaxSpreadValue[spreadType] = () => 1000;
        }

        #endregion
    }
}
