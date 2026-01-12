import { Controller, Get, UseGuards } from "@nestjs/common";
import { AppService } from "./app.service";
import { ContextService } from "./core/context/context.service";
import { AuthGuard } from "./core/auth/auth.guard";

@Controller()
export class AppController {
  constructor(
    private readonly appService: AppService,
    private readonly contextService: ContextService,
  ) {}

  @Get()
  getHello(): string {
    return this.appService.getHello();
  }

  @Get("me")
  @UseGuards(AuthGuard)
  getMe() {
    return this.contextService.getContext();
  }
}
