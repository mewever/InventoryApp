import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { provideHttpClient } from '@angular/common/http';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Home } from './home/home';
import { Items } from './items/items';
import { ItemDetails } from './item-details/item-details';
import { ItemEdit } from './item-edit/item-edit';
import { ItemAdd } from './item-add/item-add';
import { ItemDelete } from './item-delete/item-delete';
import { ItemReceive } from './item-receive/item-receive';
import { ItemRemove } from './item-remove/item-remove';

@NgModule({
  declarations: [
    App,
    Home,
    Items,
    ItemDetails,
    ItemEdit,
    ItemAdd,
    ItemDelete,
    ItemReceive,
    ItemRemove
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [
    provideHttpClient(),
    provideBrowserGlobalErrorListeners()
  ],
  bootstrap: [App]
})
export class AppModule { }
