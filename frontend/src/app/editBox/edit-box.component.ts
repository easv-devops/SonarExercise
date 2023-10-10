import {Component} from "@angular/core";
import {FormBuilder, Validators} from "@angular/forms";
import {Box, ResponseDto} from "../../models";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {DataService} from "../data.service";
import {environment} from "../../environments/environment";
import {ModalController, ToastController} from "@ionic/angular";
import {ActivatedRoute} from "@angular/router";

@Component({
  template: `

    <ion-list>
      <ion-item>
        <ion-select [formControl]="editBoxForm.controls.size" data-testid="sizeInput" label="Size"
                    placeholder="Pick size">
          <ion-select-option value="small">small</ion-select-option>
          <ion-select-option value="medium">medium</ion-select-option>
          <ion-select-option value="big">big</ion-select-option>
          <ion-select-option value="large">large</ion-select-option>
        </ion-select>
      </ion-item>
      <ion-item>
        <ion-input [formControl]="editBoxForm.controls.weight" type="number" data-testid="weightInput"
                   label="Weight of the box">
        </ion-input>
      </ion-item>
      <ion-item>
        <ion-input [formControl]="editBoxForm.controls.price" type="number" data-testid="priceInput"
                   label="Price of the box">
        </ion-input>
      </ion-item>
      <ion-item>
        <ion-select [formControl]="editBoxForm.controls.material" data-testid="materialInput" label="Material"
                    placeholder="Pick material">
          <ion-select-option value="paper">paper</ion-select-option>
          <ion-select-option value="plastic">plastic</ion-select-option>
          <ion-select-option value="metal">metal</ion-select-option>
          <ion-select-option value="wood">wood</ion-select-option>
        </ion-select>
      </ion-item>
      <ion-item>
        <ion-select [formControl]="editBoxForm.controls.color" data-testid="colorInput" label="Color"
                    placeholder="Pick color">
          <ion-select-option value="clear">clear</ion-select-option>
          <ion-select-option value="red">red</ion-select-option>
          <ion-select-option value="blue">blue</ion-select-option>
          <ion-select-option value="green">green</ion-select-option>
        </ion-select>
      </ion-item>
      <ion-item>
        <ion-input [formControl]="editBoxForm.controls.quantity" type="number" data-testid="quantityInput"
                   label="Quantity">

        </ion-input>
      </ion-item>

      <ion-item>
        <ion-button data-testid="submit" [disabled]="editBoxForm.invalid" (click)="submit()">Update Box
        </ion-button>
      </ion-item>
    </ion-list>

  `
})

export class EditBoxComponent {

  box: Box | undefined;

  editBoxForm = this.fb.group({
    size: ['', Validators.required, Validators.pattern('(?:small|medium|big|large)')],
    weight: ['', Validators.required],
    price: ['', Validators.required],
    material: ['', Validators.required, Validators.pattern('(?:paper|metal|plastic|wood)')],
    color: ['', Validators.required, Validators.pattern('(?:clear|red|blue|green)')],
    quantity: ['', Validators.required]
  })

  constructor(private activatedRoute: ActivatedRoute, public fb: FormBuilder, public modalController: ModalController, public http: HttpClient, public dataService: DataService, public toastController: ToastController) {
  }

  async submit() {

    try {
      this.activatedRoute.params.subscribe(async (params) => {
        const boxId = params['boxId'];
        if (boxId) {
          const call = this.http.put<Box>(environment.baseUrl + '/api/boxes/' + boxId, this.editBoxForm.value);
          this.box = await firstValueFrom<Box>(call);
        } else {

        }
      });


      const toast = await this.toastController.create({
        message: 'Box was updated!',
        duration: 1233,
        color: "success"
      })
      toast.present();
      this.modalController.dismiss();
    } catch (e) {
      if (e instanceof HttpErrorResponse) {
        const toast = await this.toastController.create({
          message: e.error.messageToClient,
          color: "danger"
        });
        toast.present();
      }
    }


  }
}
