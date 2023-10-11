import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import {BoxesPage} from "./home/home.page";
import {HttpClientModule} from "@angular/common/http";
import {CreateBoxComponent} from "./createBox/create-box.component";
import {ReactiveFormsModule} from "@angular/forms";
import {BoxInfoComponent} from "./box-info/box-info.component";
import {EditBoxComponent} from "./editBox/edit-box.component";

@NgModule({
  declarations: [AppComponent, BoxesPage, CreateBoxComponent, BoxInfoComponent, EditBoxComponent],
  imports: [BrowserModule, IonicModule.forRoot({mode: 'ios'}), AppRoutingModule, HttpClientModule, ReactiveFormsModule],
  providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy }],
  bootstrap: [AppComponent],
})
export class AppModule {}
