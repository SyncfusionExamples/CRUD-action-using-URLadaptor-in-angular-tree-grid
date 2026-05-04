import { Component, ViewChild } from '@angular/core';
import { TreeGridComponent, ToolbarItems, EditSettingsModel } from '@syncfusion/ej2-angular-treegrid';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { Ajax } from '@syncfusion/ej2-base';

import { FormsModule } from '@angular/forms';

import { FilterService, GridAllModule, SortService,  GroupService } from '@syncfusion/ej2-angular-grids';

import { RowDDService, ToolbarService, TreeGridAllModule, EditService, PageService } from '@syncfusion/ej2-angular-treegrid';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
   standalone:true,
    providers: [EditService, SortService, ToolbarService, RowDDService, FilterService, PageService],
    imports: [ FormsModule, GridAllModule, TreeGridAllModule],
})
export class AppComponent {
  @ViewChild('treegrid')
  public treegrid?: TreeGridComponent;
  public data?: DataManager;
  public editSettings?: EditSettingsModel;
  public toolbar?: ToolbarItems[];
  public pagesettings?: object;

  ngOnInit(): void {
    this.data = new DataManager({
      url: '/Home/UrlDatasource1',
      updateUrl: '/Home/Update',
      insertUrl: '/Home/Insert',
      removeUrl: '/Home/Remove',
      adaptor: new UrlAdaptor()
    });
    this.pagesettings = { pageSize: 4 };
    this.editSettings = { allowEditing: true, allowAdding: true, allowDeleting: true, };
    this.toolbar = ['Add', 'Edit', 'Delete', 'Update', 'Cancel', 'Search'];

  }
}
