var
    webpack = require('webpack'),
    path = require('path'),
    ExtractTextPlugin = require("extract-text-webpack-plugin"),
    HtmlWebpackPlugin = require("html-webpack-plugin"),
    HtmlWebpackPolyfillIOPlugin = require('html-webpack-polyfill-io-plugin');
    

module.exports = {
    entry: {
        //app: [
        //    './Resources/Scripts/app.ts'
        //],
        style: [
            './Resources/Styles/style.scss',
            './Resources/Scripts/app.ts',
            './Resources/Scripts/pages/basket.ts'
        ],
        //contact: [
        //    './Resources/Scripts/Pages/contact.ts'
        //]
    },
    devtool: 'inline-source-map',
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, 'wwwroot'),
        publicPath: "/",
        library: "webStoreDemo",
        libraryTarget: "var"
    },
    resolve: {
        extensions: [".js", '.ts'],
        //alias: {
        //    'vue$': 'vue/dist/vue.esm.js' // 'vue/dist/vue.common.js' for webpack 1
        //}
    },
    module: {
        loaders: [
            {
                test: /\.(sass|scss)$/,
                loader: ExtractTextPlugin.extract(['css-loader', 'sass-loader'])
            },
            {
                test: /\.(png|svg|jpg|gif)$/,
                use: [
                    'file-loader'
                ]
            },
            {
                test: /\.(woff|woff2|eot|ttf|otf)$/,
                use: [
                    'file-loader'
                ]
            },
            {
                test: /\.tsx?$/,
                loader: 'ts-loader',
                exclude: /node_modules/,
            }
        ]
    },
    plugins: [
        new ExtractTextPlugin('[name].css', { allChunks: true }),
        new HtmlWebpackPlugin({
            inject: 'body',
            filename: '../Views/Shared/_Layout.cshtml',
            template: './Views/Shared/_Layout_Template.cshtml',
            hash: true,
            files: {
                css: ['styles.css'],
                js: ['app.js']
            },
            chucks: {
                head: {
                    css: ['styles.css']
                },
                main: {
                    js: ['app.js']
                }
            }
        }),
        new HtmlWebpackPolyfillIOPlugin({
            minify: true,
            features: [
                'Element.prototype.closest'
                ]
        })
    ]
};