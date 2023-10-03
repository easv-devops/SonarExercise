import {Injectable} from "@angular/core";
import {Box} from "../models";

@Injectable({
  providedIn: 'root'
})
export class DataService {

  public boxes: Box[] = [];
  public currentBox: Box = {};
}
