export interface CostCenterTreeModel {
    costCenterId: number;
    costCenterNumber: string;
    nameAR: string;
    nameEN: string;
    parentId: number;
    costLevel: number | null;
    isActive: boolean | null;
    isLocked: boolean | null;
    isParent: boolean | null;
    isPost: boolean | null;
    isExpences: number | null;
    displayOrder: number | null;
    isSelected: boolean;
    isDeleteAction: boolean;
    children: CostCenterTreeModel[];
}