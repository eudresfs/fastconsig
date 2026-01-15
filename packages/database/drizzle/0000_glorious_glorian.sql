CREATE TABLE IF NOT EXISTS "tenants" (
	"id" text PRIMARY KEY NOT NULL,
	"clerk_org_id" text NOT NULL,
	"name" text NOT NULL,
	"cnpj" text NOT NULL,
	"slug" text NOT NULL,
	"active" boolean DEFAULT true NOT NULL,
	"created_at" timestamp DEFAULT now() NOT NULL,
	"updated_at" timestamp DEFAULT now() NOT NULL,
	CONSTRAINT "tenants_clerk_org_id_unique" UNIQUE("clerk_org_id"),
	CONSTRAINT "tenants_cnpj_unique" UNIQUE("cnpj"),
	CONSTRAINT "tenants_slug_unique" UNIQUE("slug")
);
--> statement-breakpoint
CREATE TABLE IF NOT EXISTS "audit_trails" (
	"id" text PRIMARY KEY NOT NULL,
	"tenant_id" text,
	"actor_id" text NOT NULL,
	"action" text NOT NULL,
	"resource" text NOT NULL,
	"details" text,
	"created_at" timestamp DEFAULT now() NOT NULL
);
--> statement-breakpoint
CREATE TABLE IF NOT EXISTS "tenant_configurations" (
	"id" varchar(30) PRIMARY KEY NOT NULL,
	"tenant_id" varchar(30) NOT NULL,
	"standard_margin_basis_points" integer DEFAULT 3000 NOT NULL,
	"benefit_card_margin_basis_points" integer DEFAULT 500 NOT NULL,
	"payroll_cutoff_day" integer DEFAULT 20 NOT NULL,
	"min_installment_value_cents" integer DEFAULT 1000 NOT NULL,
	"max_installments" integer DEFAULT 96 NOT NULL,
	"created_at" timestamp DEFAULT now() NOT NULL,
	"updated_at" timestamp DEFAULT now() NOT NULL
);
--> statement-breakpoint
CREATE INDEX IF NOT EXISTS "tenant_configurations_tenant_id_idx" ON "tenant_configurations" ("tenant_id");--> statement-breakpoint
DO $$ BEGIN
 ALTER TABLE "audit_trails" ADD CONSTRAINT "audit_trails_tenant_id_tenants_id_fk" FOREIGN KEY ("tenant_id") REFERENCES "tenants"("id") ON DELETE no action ON UPDATE no action;
EXCEPTION
 WHEN duplicate_object THEN null;
END $$;
--> statement-breakpoint
DO $$ BEGIN
 ALTER TABLE "tenant_configurations" ADD CONSTRAINT "tenant_configurations_tenant_id_tenants_id_fk" FOREIGN KEY ("tenant_id") REFERENCES "tenants"("id") ON DELETE no action ON UPDATE no action;
EXCEPTION
 WHEN duplicate_object THEN null;
END $$;
