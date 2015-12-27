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


using KramaxReloadExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PB_UL
{
    public static class ULU
    {
        public enum TransferType
        {
            BurnCoast,
            SoiTransition,
        }
        public class Transfer
        {
            public TransferType transferType;
            public double startTime;
            public double endTime;
            public double deltaV;

            public CelestialBody targetBody;
            public double peri;
            public double apo;
            public double inclination;
        }
        public static List<CelestialBody> getPlanetaryHierarchy(Vessel v)
        {
            List<CelestialBody> ret = new List<CelestialBody> { v.mainBody };
            try
            {
                while (ret[0].name != "Sun")
                {
                    ret.Insert(0, ret[0].GetOrbit().referenceBody);
                } 

            }
            catch (System.Exception e)
            {
                deb("Error: " + e.Message);
            }
            return ret;
        }

        public static List<CelestialBody> getCommonParentBodies(List<CelestialBody> l1, List<CelestialBody> l2)
        {
            int i = 0;
            try
            {
                while (l1[i] == l2[i])
                {
                    i++;
                }

            }
            catch (System.Exception e)
            {
                if (!(e is ArgumentOutOfRangeException))
                {
                    deb("Unknown Exception in UniversalLogistics.ULU.getCommonParentBodies: " + e.Message);
                }
            }
            return l1.GetRange(0, i);
        }

        public static CelestialBody getBodyOfTransfer(Vessel v1, Vessel v2)
        {
            return getBodyOfTransfer(getPlanetaryHierarchy(v1), getPlanetaryHierarchy(v2));
        }
        public static CelestialBody getBodyOfTransfer(List<CelestialBody> cbl1, List<CelestialBody> cbl2)
        {
            return getCommonParentBodies(cbl1, cbl2).Last();
        }
        public static List<CelestialBody> removeNonTransferBodies(List<CelestialBody> bodies, CelestialBody upTo)
        {
            while (bodies[0] != upTo)
            {
                bodies.RemoveAt(0);
            }
            bodies.RemoveAt(0);
            return bodies;
        }

        public static List<Transfer> getTransfertoEscape(double semiMajor, double inclination, CelestialBody body, double startTime)
        {
            List<Transfer> ret = new List<Transfer>();

            // Burn & Coast
            double v = Math.Sqrt(body.gravParameter / semiMajor);
            double escapeVelocity = Math.Sqrt(2 * body.gravParameter / semiMajor);
            Transfer transferBurnCoast = new Transfer(
                );
            transferBurnCoast.transferType = TransferType.BurnCoast;
            transferBurnCoast.startTime = startTime;
            transferBurnCoast.transferType = TransferType.BurnCoast;
            transferBurnCoast.deltaV = Math.Max(escapeVelocity - v, 0);
            transferBurnCoast.apo = body.sphereOfInfluence;
            transferBurnCoast.peri = semiMajor;
            transferBurnCoast.inclination = inclination;
            transferBurnCoast.endTime = startTime + 2*Math.PI * Math.Sqrt(Math.Pow(semiMajor,3)/body.gravParameter);
            ret.Add(transferBurnCoast);

/*            // Coast
            Transfer transferCoast = new Transfer();
            transferCoast.transferType = TransferType.Coast;
            transferCoast.peri = 0000;
            transferCoast.apo = 0000;
            Orbit transferOrbit = 
            transferCoast.startTime = startTime;
            transferCoast.endTime = startTime + transferOrbit.period / 2;
            ret.Add(transferCoast);
            */

            /*
            // SoiTransition
            Transfer transferSoi = new Transfer();
            transferSoi.transferType = TransferType.SoiTransition;
            transferSoi.targetBody = o.referenceBody.orbit.referenceBody;
            transferSoi.startTime = transferCoast.endTime;
            ret.Add(transferSoi);
            */

            return ret;
        }

        /*
        public static TransferOld getHohmannTransferDeltaV(CelestialBody body, Orbit o1, Orbit o2)
        {
            double v1 = Math.Sqrt(body.gravParameter / o1.semiMajorAxis);
            double v2 = Math.Sqrt(body.gravParameter / o2.semiMajorAxis);
            double ta = (o1.semiMajorAxis + o2.semiMajorAxis) / 2;
            double tv1 = Math.Sqrt(body.gravParameter * (2 / o1.semiMajorAxis - 1 / ta));
            double tv2 = Math.Sqrt(body.gravParameter * (2 / o2.semiMajorAxis - 1 / ta));

            double deltaInc = Math.Abs(o1.inclination - o2.inclination);
            if (deltaInc > Math.PI)
            {
                deltaInc = 2*Math.PI - deltaInc;
            }
            double lowerV = Math.Min(tv1, tv2);

            TransferOld t = new TransferOld();
            t.deltaV = Math.Abs(v1 - tv1) + Math.Abs(v2 - tv2) + lowerV * Math.Sqrt(Math.Pow((1 - Math.Cos(deltaInc)), 2d) + Math.Pow(Math.Sin(deltaInc), 2d));
            t.time = Math.PI * Math.Sqrt(Math.Pow(o1.semiMajorAxis + o2.semiMajorAxis,3)/8/body.gravParameter);
            return t;
        }

        public static TransferOld getTransfer(Vessel v1, Vessel v2)
        {
            List<CelestialBody> planetaryHierarchy1 = getPlanetaryHierarchy(v1);
            List<CelestialBody> planetaryHierarchy2 = getPlanetaryHierarchy(v2);
            List<CelestialBody> commonPlanetaryHierarchy = getCommonParentBodies(planetaryHierarchy1, planetaryHierarchy2);
            CelestialBody central = getBodyOfTransfer(planetaryHierarchy1, planetaryHierarchy2);
            planetaryHierarchy1 = removeNonTransferBodies(planetaryHierarchy1, central);
            planetaryHierarchy2 = removeNonTransferBodies(planetaryHierarchy2, central);

            Orbit o1 = planetaryHierarchy1.Count == 0 ? v1.orbit : planetaryHierarchy1[0].orbit;
            Orbit o2 = planetaryHierarchy2.Count == 0 ? v2.orbit : planetaryHierarchy2[0].orbit;
            TransferOld t = getHohmannTransferDeltaV(central, o1, o2);
            foreach ( List<CelestialBody> cbl in new List<List<CelestialBody>> { planetaryHierarchy1, planetaryHierarchy2 } )
            {
                for (int i = 0; i < cbl.Count; i++)
                {
                    Orbit o = i + 1 < cbl.Count ? cbl[i + 1].orbit : v1.orbit;
                    TransferOld temp = getTransfertoEscape(cbl[i], o);
                    t.deltaV += temp.deltaV;
                }
            }

            // account for waiting period until the first burn. It is only an approximation
            // when the two orbits are nearly identical, then the time is much longer but most likely not an application for UniversalLogistics
            t.time += Math.Min(o1.period, o2.period);

            return t;
        }
        */
        public static void deb(string e)
        {
            Debug.Log(e);
        }
    }

    /*
#if DEBUG
    
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class ULLoadDefaultVessel : ReloadableMonoBehaviour
    {
        public void Start()
        {
            HighLogic.SaveFolder = "default";
            Game game = GamePersistence.LoadGame("persistent", HighLogic.SaveFolder, true, false);

            if (game == null)
            {
                return;
            }

            HighLogic.CurrentGame = game;

            int vesselId = 0;
            while (vesselId < game.flightState.protoVessels.Count)
            {
                if (game.flightState.protoVessels[vesselId].vesselType == VesselType.SpaceObject ||
                    game.flightState.protoVessels[vesselId].vesselType == VesselType.Unknown)
                {
                    vesselId++;
                }
                else
                {
                    FlightDriver.StartAndFocusVessel(game, vesselId);
                    return;
                }
            }
        }
    }
#endif
     * */
}
