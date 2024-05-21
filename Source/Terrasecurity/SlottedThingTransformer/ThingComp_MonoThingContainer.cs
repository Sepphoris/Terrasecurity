﻿using HarmonyLib;
using RimWorld;
using RimWorld.BaseGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Terrasecurity
{
    public class ThingComp_MonoThingContainer : ThingComp_AutoHaulThingContainer
    {
        ThingDef currentlyAcceptedThingDef;
        public ThingDef CurrentlyAcceptedThingDef => currentlyAcceptedThingDef;
        public bool canCurrentlyReceiveThings = true;

        protected override ThingRequest ThingRequest => currentlyAcceptedThingDef == null ? ThingRequest.ForUndefined() : ThingRequest.ForDef(currentlyAcceptedThingDef);

        public override bool Accepts(Thing thing)
        {
            if(currentlyAcceptedThingDef == null)
            {
                return false;
            }
            if (!base.Accepts(thing))
            {
                return false;
            }
            if(currentlyAcceptedThingDef != thing.def)
            {
                return false;
            }
            return true;
        }

        public void SetAcceptedThingDef(ThingDef def)
        {
            currentlyAcceptedThingDef = def;
        }

        public override void PostExposeData()
        {
            Scribe.EnterNode(nameof(ThingCompProperties_MonoThingContainer));
            base.PostExposeData();
            Scribe_Defs.Look(ref currentlyAcceptedThingDef, nameof(currentlyAcceptedThingDef));
            Scribe_Values.Look(ref canCurrentlyReceiveThings, nameof(canCurrentlyReceiveThings));
            Scribe.ExitNode();
        }

        public override AcceptanceReport ShouldFill(Pawn pawn)
        {
            AcceptanceReport baseReport = base.ShouldFill(pawn);
            if (!baseReport)
            {
                return baseReport.Reason;
            }
            if(currentlyAcceptedThingDef == null)
            {
                return "NoThingAccepted";
            }
            return true;
        }
    }
}