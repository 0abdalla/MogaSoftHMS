export interface AccountTreeModel {
    accountId: number;
    accountNumber: string;
    parentAccountId: number;
    isReadOnly: boolean;
    isGroup: boolean;
    isSelected: boolean;
    accountLevel: number | null;
    accountTypeId: number | null;
    currencyTypeId: number | null;
    preCredit: number | null;
    preDebit: number | null;
    nameAR: string;
    nameEN: string;
    assetType: string;
    descriptionMethod: string;
    isDisToCostCenter:boolean;
    costCenterId: number | null;
    isActive:boolean;
    children: AccountTreeModel[];
    isDeleteAction:boolean;
}