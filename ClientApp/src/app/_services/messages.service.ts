import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MessagesService {
  private idx = 0;
  Messages$: BehaviorSubject<Message[]> = new BehaviorSubject<Message[]>([]);
  constructor() {
    this.addMessage(new Message(MessageLevel.Info, 'Test info', 100));
    this.addMessage(new Message(MessageLevel.Error, 'Test error', 10));
    this.addMessage(new Message(MessageLevel.Success, 'Test success', 10));
  }
  addMessage(msg: Message) {
    msg.id = this.idx++;
    this.Messages$.next([...this.Messages$.value, msg]);
    setTimeout(() => this.dismissMessage(msg), 1000 * msg.TimeOutSeconds);
  }
  dismissMessage(msg: Message) {
    this.Messages$.next(this.Messages$.value.filter((m) => m.id !== msg.id));
  }
  addError(msg: string, visibleSeconds: number | null) {
    this.addMessage(new Message(MessageLevel.Error, msg, visibleSeconds ?? 10));
  }
  addSuccess(msg: string) {
    this.addMessage(new Message(MessageLevel.Success, msg, 10));
  }
}

export class Message {
  id: number = 0;
  level: MessageLevel = MessageLevel.Info;
  text: string;
  TimeOutSeconds: number;
  createDate: Date;
  constructor(type: MessageLevel, text: string, timeOutSeconds: number) {
    this.text = text;
    this.TimeOutSeconds = timeOutSeconds;
    this.level = type;
    this.createDate = new Date();
  }
}
export enum MessageLevel {
  Error,
  Info,
  Success,
}
