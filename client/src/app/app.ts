import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { createClient } from "@connectrpc/connect";
import { createGrpcWebTransport } from "@connectrpc/connect-web";
import { ActivityService, FetchActivitiesRequestSchema } from '../gen/timesheet_manager_pb';
import { create } from '@bufbuild/protobuf';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  protected title = 'app';

  async ngOnInit(): Promise<void> {
    const transport = createGrpcWebTransport({
      baseUrl: "http://localhost:5203",
    });
    const client = createClient(ActivityService, transport);
    const request = create(FetchActivitiesRequestSchema);
    const response = await client.fetch(request);
    console.log(response);
  }
}
