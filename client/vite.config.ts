import { sveltekit } from "@sveltejs/kit/vite";
import { vitePreprocess } from "@sveltejs/vite-plugin-svelte";
import tailwindcss from "@tailwindcss/vite";
import adapter from "svelte-adapter-bun-next";
import { defineConfig, type ServerOptions, type UserConfig } from "vite";
import { ViteImageOptimizer } from "vite-plugin-image-optimizer";

export default defineConfig(({ mode }) => {
    const isDev = mode === "development";

    const plugins: UserConfig["plugins"] = [
        tailwindcss(),
        sveltekit({
            adapter: adapter({ precompress: true }),
            alias: {
                "@/common": "../common/src"
            },
            preprocess: vitePreprocess()
        })
    ];

    const serverOptions: ServerOptions = {
        port: 3000,
        strictPort: true,
        host: "127.0.0.1",
        proxy: {
            "/api": {
                target: "http://127.0.0.1:5096",
                changeOrigin: true,
                secure: false
            }
        }
    };

    if (!isDev) plugins.push(ViteImageOptimizer({ logStats: true }));

    return {
        server: serverOptions,
        preview: serverOptions,

        css: {
            devSourcemap: isDev
        },

        json: {
            stringify: true
        },

        plugins,

        logLevel: isDev ? "info" : "warn",
        clearScreen: false
    };
});
