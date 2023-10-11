import {Component, Input, OnInit} from '@angular/core';
import {Box} from "../../models";
import {ActivatedRoute} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {firstValueFrom} from "rxjs";
import {ModalController, ToastController} from "@ionic/angular";
import {EditBoxComponent} from "../editBox/edit-box.component";
import {DataService} from "../data.service";

@Component({
  selector: 'app-box-info',
  templateUrl: './box-info.component.html',
  styleUrls: ['./box-info.component.scss'],
})
export class BoxInfoComponent implements OnInit {

  box: Box | undefined;

  constructor(public httpClient: HttpClient, private activatedRoute: ActivatedRoute, private http: HttpClient, public modalController: ModalController,
              public toastController: ToastController, public dataService : DataService) {
    this.setId();
  }

  ngOnInit() {
    this.loadBoxInfo();
  }

  private async loadBoxInfo() {
    this.activatedRoute.params.subscribe(async (params) => {
      const boxId = params['boxId'];
      if (boxId) {
        const call = this.http.get<Box>(environment.baseUrl + '/api/boxes/' + boxId);
        this.box = await firstValueFrom<Box>(call);
      } else {

      }
    });
  }
async setId() {
    try{
      const id = (await firstValueFrom(this.activatedRoute.paramMap)).get('boxId');
      this.dataService.currentBox = (await firstValueFrom(this.httpClient.get<any>(environment.baseUrl + '/api/boxes/' + id)));
    } catch (e) {
      console.log(e);
      console.log(this.dataService.currentBox.id);
    }

}

  async openEditModal() {
    const modal = await this.modalController.create({
      component: EditBoxComponent,
    });
    modal.present();
  }
}
