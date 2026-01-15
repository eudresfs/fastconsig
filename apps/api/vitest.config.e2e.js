"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const config_1 = require("vitest/config");
const unplugin_swc_1 = __importDefault(require("unplugin-swc"));
exports.default = (0, config_1.defineConfig)({
    test: {
        include: ['test/**/*.e2e-spec.ts'],
        globals: true,
        root: './',
        environment: 'node',
    },
    plugins: [
        unplugin_swc_1.default.vite({
            module: { type: 'es6' },
        }),
    ],
    resolve: {
        alias: {
            '@fast-consig/shared': '../../packages/shared/src',
        },
    },
});
