import postgres from "postgres";
import * as schema from "./schema";
export declare const connectionString: string;
export declare const client: postgres.Sql<{}>;
export declare const db: import("node_modules/drizzle-orm/postgres-js").PostgresJsDatabase<typeof schema>;
export * from "drizzle-orm";
export * from "./schema";
//# sourceMappingURL=index.d.ts.map