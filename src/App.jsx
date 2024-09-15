
import React, { useState } from 'react'
import Login from './Component/Login';
import Signup from './Component/Signup';


function App() {

  const [isLogin, setIsLogin] = useState(true);

  function handleChangePage(text) {
    setIsLogin(text);
  }

  function page() {

    if (isLogin) {
      return <Login onChangePage={handleChangePage} />;
    }
    return <Signup onChangePage={handleChangePage} />;
  }

  return (
    <>
      {page()}
    </>

  )

}

export default App
