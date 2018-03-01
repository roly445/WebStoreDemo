module.exports = {
    resolve: {
        // For modules referenced with no filename extension, Webpack will consider these extensions
        extensions: ['', '.js', '.ts']
    },
    module: {
        loaders: [
            // This example only configures Webpack to load .ts files. You can also drop in loaders
            // for other file types, e.g., .coffee, .sass, .jsx, ...
            { test: /\.ts$/, loader: 'ts-loader' }
        ]
    },
    entry: {
        // The loader will follow all chains of reference from this entry point...
        main: ['./ClientApp/MyApp.ts']
    },
    output: {
        // ... and emit the built result in this location
        path: __dirname + '/wwwroot/dist',
        filename: '[name].js'
    },
};