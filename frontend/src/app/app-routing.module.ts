import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import {BoxesPage} from "./home/home.page";

const routes: Routes = [
  {
    path: '',
    redirectTo: 'boxes',
    pathMatch: 'full'
  },
  {
    path: 'boxes',
    component: BoxesPage
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
