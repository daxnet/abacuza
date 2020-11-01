import { NbMenuItem } from '@nebular/theme';

export const MENU_ITEMS: NbMenuItem[] = [
  {
    title: 'FEATURES',
    group: true,
  },
  {
    title: 'Projects',
    icon: 'cube-outline',
    children: [
      {
        title: 'Projects',
        icon: 'settings-2-outline',
        link: '/pages/projects/project-list',
      }
    ],
  },
  {
    title: 'Jobs',
    icon: 'cube-outline',
    children: [
      {
        title: 'Job Runners',
        icon: 'settings-2-outline',
        link: '/pages/jobs/job-runners',
      },
    ],
  },
  {
    title: 'Clusters',
    icon: 'layers-outline',
    children: [
      {
        title: 'Connections',
        icon: 'grid-outline',
        link: '/pages/clusters/connections',
      },
      {
        title: 'Types',
        icon: 'grid-outline',
        link: '/pages/clusters/types',
      },
    ],
  },
  {
    title: 'Miscellaneous',
    icon: 'shuffle-2-outline',
    children: [
      {
        title: '404',
        link: '/pages/miscellaneous/404',
      },
    ],
  },
  {
    title: 'Auth',
    icon: 'lock-outline',
    children: [
      {
        title: 'Login',
        link: '/auth/login',
      },
      {
        title: 'Register',
        link: '/auth/register',
      },
      {
        title: 'Request Password',
        link: '/auth/request-password',
      },
      {
        title: 'Reset Password',
        link: '/auth/reset-password',
      },
    ],
  },
];
