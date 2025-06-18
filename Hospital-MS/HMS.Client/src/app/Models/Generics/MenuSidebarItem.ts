  export class MenuSidebarItem {
    displayName?: string;
    route?: string;
    icon?: string;
    isGroup?: boolean;
    subMenus?: MenuSidebarItem[] = [];
  }