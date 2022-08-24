namespace I2.Loc
{
	public static class ScriptLocalization
	{

		public static string Power 		{ get{ return LocalizationManager.GetTranslation ("Power"); } }
		public static string Interval 		{ get{ return LocalizationManager.GetTranslation ("Interval"); } }
		public static string Sec 		{ get{ return LocalizationManager.GetTranslation ("Sec"); } }
		public static string Min 		{ get{ return LocalizationManager.GetTranslation ("Min"); } }
		public static string Buy 		{ get{ return LocalizationManager.GetTranslation ("Buy"); } }
		public static string Level 		{ get{ return LocalizationManager.GetTranslation ("Level"); } }
	}

    public static class ScriptTerms
	{

		public const string Power = "Power";
		public const string Interval = "Interval";
		public const string Sec = "Sec";
		public const string Min = "Min";
		public const string Buy = "Buy";
		public const string Level = "Level";
	}
}