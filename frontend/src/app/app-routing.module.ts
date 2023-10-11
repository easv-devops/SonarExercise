import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import {BoxesPage} from "./home/home.page";
import {BoxInfoComponent} from "./box-info/box-info.component";


const routes: Routes = [
  {
    path: '',
    redirectTo: 'boxes',
    pathMatch: 'full'
  },
  {
    path: 'boxes',
    component: BoxesPage
  },
  {
    path: 'box-info/:boxId',
    component: BoxInfoComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
