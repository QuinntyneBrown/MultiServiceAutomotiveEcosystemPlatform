import type { StorybookConfig } from '@storybook/angular';

const config: StorybookConfig = {
  "stories": [
    "../src/**/*.mdx",
    "../src/**/*.stories.@(js|jsx|mjs|ts|tsx)"
  ],
  "addons": [
    "@storybook/addon-a11y",
    "@storybook/addon-docs",
    "@storybook/addon-onboarding"
  ],
  "framework": "@storybook/angular",
  webpackFinal: async (config) => {
    // Add CSS loader support
    config.module = config.module || {};
    config.module.rules = config.module.rules || [];
    
    // Find and update the CSS rule
    const cssRule = config.module.rules.find((rule: any) => 
      rule.test?.toString().includes('css')
    );
    
    if (!cssRule) {
      config.module.rules.push({
        test: /\.css$/,
        use: ['style-loader', 'css-loader'],
      });
    }
    
    return config;
  },
};
export default config;