import {Component} from "@angular/core";
import {FormBuilder, Validators} from "@angular/forms";
import {Box, ResponseDto} from "../../models";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {State} from "../../state";
import {firstValueFrom} from "rxjs";
import {environment} from "../../environments/environment";
import {ModalController, ToastController} from "@ionic/angular";

@Component({
  template: `

    <ion-list>
    <ion-item>
      <ion-input [formControl]="createNewBoxForm.controls.size" data-testid="sizeInput" label="Size of the box">

      </ion-input>
      <div *ngIf="!createNewBoxForm.controls.size.valid">Must be proper size! (small, medium, big or large)</div>
    </ion-item>
    <ion-item>
      <ion-input [formControl]="createNewBoxForm.controls.weight" data-testid="weightInput"  label="Weight of the box">

      </ion-input>
    </ion-item>
    <ion-item>
      <ion-input [formControl]="createNewBoxForm.controls.price" data-testid="priceInput"  label="Price of the box">

      </ion-input>
    </ion-item>
    <ion-item>
      <ion-input  [formControl]="createNewBoxForm.controls.material"  data-testid="materialInput"   label="Material of the box">

      </ion-input>
    </ion-item>
    <ion-item>
      <ion-input [formControl]="createNewBoxForm.controls.color" data-testid="colorInput"  label="Color of the box">

      </ion-input>
    </ion-item>
    <ion-item>
      <ion-input [formControl]="createNewBoxForm.controls.quantity" data-testid="quantityInput"  label="Quantity">

      </ion-input>
    </ion-item>

    <ion-item>
      <ion-button data-testid="submit" [disabled]="createNewBoxForm.invalid" (click)="submit()">create</ion-button>
    </ion-item>
  </ion-list>

  `
})

export class CreateBoxComponent {

  createNewBoxForm = this.fb.group({
    size: ['', Validators.required],
    weight: ['', Validators.required],
    price: ['', Validators.required],
    material: ['', Validators.required],
    color: ['', Validators.required],
    quantity: ['', Validators.required]
  })

  constructor(public fb: FormBuilder, public modalController: ModalController, public http: HttpClient, public state: State, public toastController: ToastController) {
  }

  async submit() {

    try {
      const observable =     this.http.post<ResponseDto<Box>>(environment.baseUrl + '/api/boxes', this.createNewBoxForm.getRawValue())

      const response = await firstValueFrom(observable);
      this.state.books.push(response.responseData!);

      const toast = await this.toastController.create({
        message: '????????',
        duration: 1233,
        color: "success"
      })
      toast.present();
      this.modalController.dismiss();
    } catch (e) {
      if(e instanceof HttpErrorResponse) {
        const toast = await this.toastController.create({
          message: e.error.messageToClient,
          color: "danger"
        });
        toast.present();
      }
    }


  }
}
