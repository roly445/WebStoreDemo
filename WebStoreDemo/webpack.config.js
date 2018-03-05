var
    webpack = require('webpack'),
    path = require('path'),
    ExtractTextPlugin = require("extract-text-webpack-plugin");
    //CopyWebpackPlugin = require("copy-webpack-plugin"),
    //CompressionPlugin = require("compression-webpack-plugin"),
    //OptimizeCssAssetsPlugin = require('optimize-css-assets-webpack-plugin'),
    //CleanWebpackPlugin = require('clean-webpack-plugin');

module.exports = {
    entry: {
        //app: [
        //    './Resources/Scripts/app.ts'
        //],
        style: [
            './Resources/Styles/style.scss'
        ],
        //contact: [
        //    './Resources/Scripts/Pages/contact.ts'
        //]
    },
    devtool: 'inline-source-map',
    output: {
        path: path.resolve(__dirname, 'wwwroot'),
        filename: "[name].js"
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
        //new CleanWebpackPlugin(['wwwroot']),
        new ExtractTextPlugin("[name].css",
            {
                allChunks: true
            }),
        //new OptimizeCssAssetsPlugin({
        //    assetNameRegExp: /\.css$/g,
        //    cssProcessorOptions: { discardComments: { removeAll: true } }
        //  }),
        //new CompressionPlugin({
        //     //algorithm: gzipMaxCompression,
        //     regExp: /\.(js|css)$/,
        //     minRatio: 0
        //   })
        // ,
        //new CopyWebpackPlugin([
        //    {
        //        from: './Resources/Images/logo-home.png', to: './images/logo-home.png'
        //    }, {
        //        from: './Resources/Images/logo-small.png', to: './images/logo-small.png'
        //    }
        //])
    ]
};