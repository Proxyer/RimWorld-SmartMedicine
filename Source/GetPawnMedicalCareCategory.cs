﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Verse;
using RimWorld;
using HarmonyLib;

namespace SmartMedicine
{
	[StaticConstructorOnStartup]
	static class GetPawnMedicalCareCategory
	{
		//        public static MedicalCareCategory TendAdvice(Pawn patient) 
		public delegate MedicalCareCategory TendAdviceDel(Pawn patient);
		public static TendAdviceDel TendAdvice;

		static GetPawnMedicalCareCategory()
		{
			try
			{
				//Harmony 2.2.1
				TendAdvice = AccessTools.MethodDelegate<TendAdviceDel>("Pharmacist.PharmacistUtility:TendAdvice");
			}
			catch (Exception)
			{ //Well you dont have pharmacist then}
			}
		}

		public static MedicalCareCategory GetCare(this Pawn pawn)
		{
			if (TendAdvice != null)
			{
				var care = TendAdvice(pawn);
				Log.Message($"Pharmacist tend advicefor {pawn} is {care}");
				return care;
			}
			return pawn.playerSettings?.medCare ?? MedicalCareCategory.Best;
		}
	}
}
