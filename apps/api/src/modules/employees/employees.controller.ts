import { Controller, Get, Post, Put, Delete, Body, Param, Query, UseGuards, UsePipes } from '@nestjs/common';
import { ClerkAuthGuard } from '../../core/auth/clerk-auth.guard';
import { ZodValidationPipe } from '../../shared/pipes/zod-validation.pipe';
import { EmployeesService } from './employees.service';
import { CreateEmployeeDto, UpdateEmployeeDto } from './dto';
import { createEmployeeSchema, updateEmployeeSchema } from '@fast-consig/shared';

@Controller('api/v1/employees')
@UseGuards(ClerkAuthGuard)
export class EmployeesController {
  constructor(private readonly employeesService: EmployeesService) {}

  @Post()
  @UsePipes(new ZodValidationPipe(createEmployeeSchema))
  async create(@Body() dto: CreateEmployeeDto) {
    const employee = await this.employeesService.create(dto);
    return { success: true, data: employee };
  }

  @Get()
  async findAll(@Query('limit') limit?: string, @Query('offset') offset?: string) {
    const employees = await this.employeesService.findAll(
      limit ? parseInt(limit) : 50,
      offset ? parseInt(offset) : 0,
    );
    return { success: true, data: employees };
  }

  @Get('cpf/:cpf')
  async findByCpf(@Param('cpf') cpf: string) {
    const employee = await this.employeesService.findByCpf(cpf);
    return { success: true, data: employee };
  }

  @Get(':id')
  async findById(@Param('id') id: string) {
    const employee = await this.employeesService.findById(id);
    return { success: true, data: employee };
  }

  @Put(':id')
  @UsePipes(new ZodValidationPipe(updateEmployeeSchema))
  async update(@Param('id') id: string, @Body() dto: UpdateEmployeeDto) {
    const employee = await this.employeesService.update(id, dto);
    return { success: true, data: employee };
  }

  @Delete(':id')
  async softDelete(@Param('id') id: string) {
    await this.employeesService.softDelete(id);
    return { success: true, message: 'Employee deleted successfully' };
  }
}
