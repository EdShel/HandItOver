module.exports = {
  presets: [
    '@vue/cli-plugin-babel/preset'
  ],
  plugins: [
    [
      "babel-plugin-root-import",
      {
        "paths": [
          {
            "rootPathPrefix": "~",
            "rootPathSuffix": "src"
          },
          {
            "rootPathPrefix": "@",
            "rootPathSuffix": "src/lang"
          }
        ]
      }
    ]
  ]
}
