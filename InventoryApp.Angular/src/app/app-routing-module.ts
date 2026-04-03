import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Home } from '../app/home/home';
import { Items } from '../app/items/items';
import { ItemAdd } from '../app/item-add/item-add';
import { ItemDelete } from '../app/item-delete/item-delete';
import { ItemDetails } from '../app/item-details/item-details';
import { ItemReceive } from '../app/item-receive/item-receive';
import { ItemRemove } from '../app/item-remove/item-remove';
import { ItemEdit } from '../app/item-edit/item-edit';

const routes: Routes = [
  { path: "", component: Home },
  { path: "items", component: Items },
  { path: "items/add", component: ItemAdd },
  { path: "items/delete/:id", component: ItemDelete },
  { path: "items/details/:id", component: ItemDetails },
  { path: "items/edit/:id", component: ItemEdit },
  { path: "items/receive/:id", component: ItemReceive },
  { path: "items/remove/:id", component: ItemRemove },
  { path: "**", redirectTo: "" }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
