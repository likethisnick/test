import * as React from 'react';
import { alpha, styled } from '@mui/material/styles';
import Box from '@mui/material/Box';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import Container from '@mui/material/Container';
import Divider from '@mui/material/Divider';
import MenuItem from '@mui/material/MenuItem';
import Drawer from '@mui/material/Drawer';
import MenuIcon from '@mui/icons-material/Menu';
import CloseRoundedIcon from '@mui/icons-material/CloseRounded';
import ColorModeIconDropdown from '../../shared-theme/ColorModeIconDropdown';
import Sitemark from '../components/SitemarkIcon';


const StyledToolbar = styled(Toolbar)(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'space-between',
  flexShrink: 0,
  padding: '8px 12px'
}));

export default function SurvefyNagivate() {
  const [open, setOpen] = React.useState(false);

  const toggleDrawer = (newOpen) => () => {
    setOpen(newOpen);
  };

  return (
    <AppBar
      position="fixed"
      enableColorOnDark
      sx={{ backgroundColor: '#0004FF' }}
    >
      <Container maxWidth="lg" sx={{ height: '64px'  }}>
        <StyledToolbar disableGutters sx={{ height: '64px' }}>
          <Box sx={{ flexGrow: 1, display: 'flex', alignItems: 'center', px: 0 }}>
            <Sitemark />
            <Box sx={{ display: { xs: 'none', md: 'flex' }, px: 5 }}>
              <Button variant="text" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' } }} size="small" >
              <a href="/">Home</a>
              </Button>
              <Button variant="text" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' } }} size="small">
                Test
              </Button>
              <Button variant="text" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' }, minWidth: 0 }} size="small">
                <a href="/blog">Blog</a>
              </Button>

            </Box>
          </Box>
          <Box
            sx={{
              display: { xs: 'none', md: 'flex' },
              gap: 1,
              alignItems: 'center',
            }}
          >
            <Button color="primary" variant="text" size="small" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' } }}>
            <a href="/login">Sign in</a>
            </Button>
            <Button color="primary" variant="outlined" size="small" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' } }}>
              Sign up
            </Button>
            {/*<ColorModeIconDropdown />*/}
          </Box>
          <Box sx={{ display: { xs: 'flex', md: 'none' }, gap: 1 }}>
            <ColorModeIconDropdown size="medium" />
            <IconButton aria-label="Menu button" onClick={toggleDrawer(true)}>
              <MenuIcon />
            </IconButton>
            <Drawer
              anchor="top"
              open={open}
              onClose={toggleDrawer(false)}
              PaperProps={{
                sx: {
                  top: 'var(--template-frame-height, 0px)',
                },
              }}
            >
              <Box sx={{ p: 2 }}>
                <Box
                  sx={{
                    display: 'flex',
                    justifyContent: 'flex-end',
                  }}
                >
                  <IconButton onClick={toggleDrawer(false)}>
                    <CloseRoundedIcon />
                  </IconButton>
                </Box>
                <MenuItem>
                  <Button variant="text" color="info" size="small" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' } }}>
                    <a href="/">Home</a>
                  </Button>
                </MenuItem>
                <MenuItem>
                  <Button variant="text" color="info" size="small" sx={{ color: 'rgb(255, 255, 255)', fontSize: 16, '&:hover': { backgroundColor: '#4c7dff' }, minWidth: 0 }}>
                    <a href="/blog">Blog</a>
                  </Button>
                </MenuItem>
                <Divider sx={{ my: 3 }} />
                <MenuItem>
                  <Button color="primary" variant="contained" fullWidth sx={{ color: 'rgb(255, 255, 255)' }}>
                    Sign up
                  </Button>
                </MenuItem>
                <MenuItem>
                  <Button color="primary" variant="outlined" fullWidth sx={{ color: 'rgb(255, 255, 255)' }}>
                    Sign in
                  </Button>
                </MenuItem>
              </Box>
            </Drawer>
          </Box>
        </StyledToolbar>
      </Container>
    </AppBar>
  );
}
