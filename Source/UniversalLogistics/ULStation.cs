/*
 * UniversalLogistics.ULStation: Governing the Station for the Universal Logistics Management System
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
/*    
    public class ULStation : PartModule
    {
        private double nextUpdate = 0;
        private double updateInterval = 5d;
        public void perform()
        {
            var now = Planetarium.GetUniversalTime();
            if (now >= nextUpdate)
            {
                nextUpdate = now + updateInterval;
                onMyRegularUpdate();
            }
        }

        private void onMyRegularUpdate()
        {
            List<CelestialBody> cb = ULU.getPlanetaryHierarchy(vessel);
            ULU.deb("---");
            cb.ForEach(c => ULU.deb("Body: " + c.name));

            List<CelestialBody> d = ULU.getCommonParentBodies(cb, cb);
            d.ForEach(c => ULU.deb("Common: " + c.name));
        }

        public override void OnStart(StartState state)
        {
            part.force_activate();
        }

        public override void OnFixedUpdate()
        {
            if (!HighLogic.LoadedSceneIsFlight)
            {
                return;
            }
            perform();
        }
    }
 * */
}
