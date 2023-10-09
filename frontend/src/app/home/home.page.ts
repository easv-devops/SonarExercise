import {Component} from '@angular/core';
import {ModalController, ToastController} from "@ionic/angular";
import {CreateBoxComponent} from "../createBox/create-box.component";
import {environment} from "../../environments/environment";
import {Box, ResponseDto} from "../../models";
import {firstValueFrom} from "rxjs";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {DataService} from "../data.service";
import {State} from "../../state";

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

      <ion-list>
    <ion-card [attr.data-testid]="'card_'+box.id" *ngFor="let box of dataService.boxes">
      <ion-card-header>
        <ion-card-title>Box number {{box.id}}</ion-card-title>
      </ion-card-header>
      <ion-list>
        <ion-item>
          <ion-label>Size: {{box.size}}</ion-label>
        </ion-item>
        <ion-item>
          <ion-label>Material: {{box.material}}</ion-label>
        </ion-item>
        <ion-item>
          <ion-label>Color: {{box.color}}</ion-label>
        </ion-item>
        <ion-item>
          <ion-label>Quantity: {{box.quantity}}</ion-label>
        </ion-item>
      </ion-list>
      <ion-button>More info</ion-button>
      <ion-button>Edit</ion-button>
      <ion-button (click)="deleteBox(box.id)">Delete</ion-button>
    </ion-card>
      </ion-list>
    </ion-content>
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
              public http: HttpClient,
              public state: State) {
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


  async deleteBox(boxId: number | undefined) {
    try {
      await firstValueFrom(this.http.delete<ResponseDto<boolean>>(environment.baseUrl + '/api/boxes/' + boxId))
      this.dataService.boxes=this.dataService.boxes.filter(a => a.id!=boxId)

      const toast = await this.toastController.create({
        message: 'the box was successfully deleted',
        duration: 1200,
        color: 'success'
      })
      toast.present();
    }  catch (error:any) {

      let errorMessage = 'Error';

      if (error instanceof HttpErrorResponse) {

        errorMessage = error.error?.message || 'Server error';
      } else if (error.error instanceof ErrorEvent) {

        errorMessage = error.error.message;
      }

      const toast = await this.toastController.create({
        color: 'danger',
        duration: 2000,
        message: errorMessage
      });

      toast.present();
    }

  }
}
