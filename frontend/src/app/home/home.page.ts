import {Component, OnInit} from '@angular/core';
import {ModalController, ToastController} from "@ionic/angular";
import {CreateBoxComponent} from "../createBox/create-box.component";
import {environment} from "../../environments/environment";
import {Box} from "../../models";
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {DataService} from "../data.service";

@Component({
  selector: 'app-home',
  template: `
    <ion-content [fullscreen]="true">

      <div id="container">
        <strong>
          <ion-icon name="cube-outline"></ion-icon>
          Box Factory
          <ion-icon name="cube-outline"></ion-icon>
        </strong>
        <br>
        <br>
        <p>Welcome, boomer boss! Ready to manage your boxes?</p>
      </div>

    </ion-content>
    <ion-card [attr.data-testid]="'card_'+box.id" *ngFor="let box of dataService.boxes" routerLink="/api/boxes/{box.id}">
      <ion-list>
        <ion-item>
          <ion-label>{{box.size}}</ion-label>
        </ion-item>
        <ion-item>
          <ion-label>Mega Man X</ion-label>
        </ion-item>
        <ion-item>
          <ion-label>The Legend of Zelda</ion-label>
        </ion-item>
        <ion-item>
          <ion-label>Pac-Man</ion-label>
        </ion-item>
        <ion-item>
          <ion-label>Super Mario World</ion-label>
        </ion-item>
      </ion-list>
    </ion-card>
    <ion-fab slot="fixed" vertical="bottom" horizontal="end">
      <ion-fab-button data-testid="createBox" (click)="openModal()">
        <ion-icon name="add"></ion-icon>
      </ion-fab-button>
    </ion-fab>
  `,
  styleUrls: ['home.page.scss'],
})
export class BoxesPage{

  constructor(public modalController: ModalController,
              public toastController: ToastController,
              public dataService: DataService,
              public http: HttpClient) {
    this.getFeedData();
  }

  async getFeedData() {
    const call = this.http.get<Box[]>(environment.baseUrl + '/api/stock');
    this.dataService.boxes = await firstValueFrom<Box[]>(call);
  }

  async openModal() {
    const modal = await this.modalController.create({
      component: CreateBoxComponent
    });
    modal.present();
  }
}
