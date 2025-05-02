export enum VisitType {
    General = 'General',
    Consultation = 'Consultation',
    Surgery = 'Surgery',
    Screening = 'Screening',
    Radiology = 'Radiology'
  }
  
  export const VisitTypeLabels: { [key in VisitType]: string } = {
    [VisitType.General]: 'كشف',
    [VisitType.Consultation]: 'استشارة',
    [VisitType.Surgery]: 'عمليات',
    [VisitType.Screening]: 'تحاليل',
    [VisitType.Radiology]: 'أشعة'
  };