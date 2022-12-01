import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'SignalRAspNetCore';
  messages: string[] = [];
  chatForm: FormGroup;
  users: string[];
  hubConnection: signalR.HubConnection;

  ngOnInit(): void {
    this.chatForm = new FormGroup({
      message: new FormControl(''),
      group: new FormControl('Everyone'),
    });

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7167/Messages')
      .build();

    this.hubConnection.on('ReceiveMessage', (msg) => {
      console.log(msg);
      this.messages.push(msg);
    });

    this.hubConnection.on('UsersConnected', (users) => {
      console.log(users);
      this.users = users;
    });

    this.hubConnection
      .start()
      .then(() => {
        console.log('connection Started ...');
      })
      .catch((error) => {
        console.log(error);
      });
  }

  onJoinGroup() {
    this.hubConnection.invoke('JoinGroup', 'PrivateGroup');
  }

  onSubmit() {
    const { message, group } = this.chatForm.value;
    if (group == 'Everyone' || group == 'Myself') {
      let method =
        group == 'Everyone' ? 'SendMessageToAll' : 'SendMessageToCaller';
      this.hubConnection.invoke(method, message);
    } else if (group != 'PrivateGroup') {
      this.hubConnection.invoke('SendMessageToUser', group, message);
    } else {
      console.log('Sending message to Group..');
      this.hubConnection.invoke('SendMessageToGroup', 'PrivateGroup', message);
    }

    console.log(message);
  }
}
