namespace Hospital_MS.Core.Enums
{
    public enum PatientStatus
    {
        Archived,
        Treated,
        CriticalCondition,
        Surgery,
        FollowUp,
        Staying,
        Outpatient, // عيادات خارجية  


        IntensiveCare, // عناية مركزة  
        Emergency, // طوارئ  
        NeonatalCare, // حضانات الأطفال  
    }
}
