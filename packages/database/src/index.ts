import { drizzle } from "drizzle-orm/postgres-js";
import postgres from "postgres";
import * as schema from "./schema";

export const connectionString =
  process.env.DATABASE_URL || "postgres://postgres:postgres@localhost:5432/fastconsig";

export const client = postgres(connectionString);
export const db = drizzle(client, { schema });

export * from "drizzle-orm";
export * from "./schema";
