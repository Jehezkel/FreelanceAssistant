import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MessagesService {
  Messages$: BehaviorSubject<Message[]> = new BehaviorSubject<Message[]>([]);
  constructor() {
    this.addMessage(new Message(MessageLevel.Info, 'Test info', 10));
    this.addMessage(new Message(MessageLevel.Error, 'Test error', 10));
    this.addMessage(new Message(MessageLevel.Success, 'Test success', 10));
  }
  addMessage(msg: Message) {
    this.Messages$.next([...this.Messages$.value, msg]);
  }
}

export class Message {
  level: MessageLevel = MessageLevel.Info;
  text: string;
  dismissTime: number;
  constructor(type: MessageLevel, text: string, dismissTime: number) {
    this.text = text;
    this.dismissTime = dismissTime;
    this.level = type;
  }
}
export enum MessageLevel {
  Error,
  Info,
  Success,
}
