export class Box {
  id?: number;
  size?: string;
  weight?: number;
  price?: number;
  material?: string;
  color?: string;
  quantity?: number
}

export class ResponseDto<T> {
  responseData?: T;
  messageToClient?: string;
}
