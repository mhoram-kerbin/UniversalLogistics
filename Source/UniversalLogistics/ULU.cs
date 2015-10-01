/*
 * UniversalLogistics.ULU: Utility functions for the Universal Logistics Management System
 * 
 * Copyright 2015 by Mhoram Kerbin
 * 
 * This file is part of UniversalLogistics.
 * 
 * UniversalLogistics is free software: you can redistribute it
 * and/or modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 * 
 * UniversalLogistics is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with TrackingStationAmbientlight.  If not, see
 * <http://www.gnu.org/licenses/>.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PB_UL
{
    public static class ULU
    {
        public static List<CelestialBody> getPlanetaryHierarchy(Vessel v)
        {
            List<CelestialBody> ret = new List<CelestialBody> { v.mainBody };
            while (ret[0].HasParent(ret[0]))
            {
                ret.Insert(0, ret[0].GetOrbit().referenceBody);
            }
            return ret;
        }

        public static void deb(string e)
        {
            Debug.Log(e);
        }
    }
}
