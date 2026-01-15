-- Create employees table
CREATE TABLE IF NOT EXISTS "employees" (
	"id" text PRIMARY KEY NOT NULL,
	"tenant_id" text NOT NULL,
	"cpf" text NOT NULL,
	"enrollment_id" text NOT NULL,
	"name" text NOT NULL,
	"email" text,
	"phone" text,
	"gross_salary" integer NOT NULL,
	"mandatory_discounts" integer DEFAULT 0 NOT NULL,
	"version" integer DEFAULT 1 NOT NULL,
	"created_at" timestamp DEFAULT now() NOT NULL,
	"updated_at" timestamp DEFAULT now() NOT NULL,
	"deleted_at" timestamp
);
--> statement-breakpoint
CREATE INDEX IF NOT EXISTS "employees_tenant_id_idx" ON "employees" ("tenant_id");
--> statement-breakpoint
CREATE UNIQUE INDEX IF NOT EXISTS "employees_tenant_cpf_unique" ON "employees" ("tenant_id","cpf");
--> statement-breakpoint
CREATE UNIQUE INDEX IF NOT EXISTS "employees_tenant_enrollment_unique" ON "employees" ("tenant_id","enrollment_id");
--> statement-breakpoint
DO $$ BEGIN
 ALTER TABLE "employees" ADD CONSTRAINT "employees_tenant_id_tenants_id_fk" FOREIGN KEY ("tenant_id") REFERENCES "tenants"("id") ON DELETE no action ON UPDATE no action;
EXCEPTION
 WHEN duplicate_object THEN null;
END $$;
