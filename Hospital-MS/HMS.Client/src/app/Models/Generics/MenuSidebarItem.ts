  export class MenuSidebarItem {
    displayName?: string;
    route?: string;
    icon?: string;
    isGroup?: boolean;
    pageName?: string;
    subMenus?: MenuSidebarItem[] = [];
  }