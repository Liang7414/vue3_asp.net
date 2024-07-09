<template>
  <div class="login-page">
    <!-- Navigation Bar -->
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark fixed-top">
      <div class="container-fluid">
        <a class="navbar-brand" href="#">Navbar</a>
        <button
          class="navbar-toggler"
          type="button"
          @click="toggleNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span class="navbar-toggler-icon"></span>
        </button>
        <div
          class="collapse navbar-collapse"
          :class="{ show: navExpanded }"
          id="navbarNav"
        >
          <ul class="navbar-nav ms-auto" :class="{ 'flex-column full-height': navExpanded }">
            <li class="nav-item">
              <a class="nav-link" href="#">Link 1</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="#">Link 2</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="#">Link 3</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="#">Link 4</a>
            </li>
          </ul>
        </div>
      </div>
    </nav>

    <!-- Login Form -->
    <div
      class="background-container d-flex justify-content-center align-items-center vh-100"
      :class="{ 'hidden-form': navExpanded }"
    >
      <div class="login-box p-4" :class="{ 'hidden-form': navExpanded }">
        <h2 class="text-center">登入</h2>
        <form @submit.prevent="handleSubmit" novalidate>
          <div class="mb-3">
            <label for="idNumber" class="form-label">身分證字號</label>
            <input
              type="text"
              class="form-control"
              id="idNumber"
              v-model="idNumber"
              :class="{ 'is-invalid': idNumberErrors.length }"
              required
            />
            <div v-if="idNumberErrors.length" class="invalid-feedback">
              <div v-for="error in idNumberErrors" :key="error">{{ error }}</div>
            </div>
          </div>
          <div class="mb-3">
            <label for="password" class="form-label">密碼</label>
            <input
              type="password"
              class="form-control"
              id="password"
              v-model="password"
              :class="{ 'is-invalid': passwordErrors.length }"
              required
            />
            <div v-if="passwordErrors.length" class="invalid-feedback">
              <div v-for="error in passwordErrors" :key="error">{{ error }}</div>
            </div>
          </div>
          <button type="submit" class="btn btn-primary w-100">登入</button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import Cookies from 'js-cookie';
import { useRouter, useRoute } from 'vue-router';
import axios from 'axios';
import { ref, onMounted, onBeforeUnmount } from 'vue';

export default {
  setup() {
    const idNumber = ref('');
    const password = ref('');
    const idNumberErrors = ref([]);
    const passwordErrors = ref([]);
    const navExpanded = ref(false);
    const router = useRouter();
    const route = useRoute();

    const validateIdNumber = () => {
      idNumberErrors.value = [];
      const idNumberPattern = /^[A-Z][12]\d{8}$/;
      if (!idNumberPattern.test(idNumber.value)) {
        idNumberErrors.value.push('請輸入有效的身分證字號');
      }
      if (!idNumber.value) {
        idNumberErrors.value.push('身分證字號是必填項');
      }
    };

    const validatePassword = () => {
      passwordErrors.value = [];
      if (password.value.length < 6) {
        passwordErrors.value.push('密碼長度至少為6個字符');
      }
      if (!password.value) {
        passwordErrors.value.push('密碼是必填項');
      }
    };

    const handleSubmit = async () => {
      validateIdNumber();
      validatePassword();
      if (idNumberErrors.value.length === 0 && passwordErrors.value.length === 0) {
        try {
          const response = await axios.post('http://localhost:5073/Login', {
            idNumber: idNumber.value,
            password: password.value,
          });
          if (response.data.success) 
          {
            alert('登入成功！');
            Cookies.set('idNumber', idNumber.value, { expires: 1 });
            Cookies.set('password', password.value, { expires: 1 });
            router.push('/vote');

            setTimeout(() => {
              Cookies.remove('idNumber');
              Cookies.remove('password');
              router.push({ path: '/', query: { expired: true } });
            }, 1 * 60 * 1000); // 10 分鐘後重定向到登入頁面
      
          } 
          else 
          {
            alert("此用戶已完成投票!!");
          }
        } catch (error) {
          console.error('Login error:', error);
          alert('找不到該用戶資料');
        }
      }
    };

    const toggleNav = () => {
      navExpanded.value = !navExpanded.value;
    };

    const handleResize = () => {
      if (window.innerWidth >= 769) {
        navExpanded.value = false;
      }
    };

    onMounted(() => {
      window.addEventListener('resize', handleResize);
      handleResize();
      if (route.query.expired) {
        alert('登入已過期，請重新登入');
      }
    });

    onBeforeUnmount(() => {
      window.removeEventListener('resize', handleResize);
    });

    return {
      idNumber,
      password,
      idNumberErrors,
      passwordErrors,
      navExpanded,
      handleSubmit,
      toggleNav,
    };
  },
};
</script>

<style>
.login-page {
  margin: 0;
  padding-top: 56px; /* Ensure the content is not hidden under the navbar */
}

.background-container {
  background-image: url('https://fastly.picsum.photos/id/0/5000/3333.jpg?hmac=_j6ghY5fCfSD6tvtcV74zXivkJSPIfR9B8w34XeQmvU');
  background-size: cover;
  background-position: center;
  height: 100vh;
  width: 100vw;
  position: absolute;
  top: 0;
  left: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: opacity 0.3s ease;
}

.login-box {
  width: 100%;
  max-width: 400px;
  padding: 2rem;
  background: rgba(255, 255, 255, 0.8);
  border-radius: 5px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  transition: opacity 0.3s ease;
}

.btn {
  background-color: #111;
  border: solid 0.5px #555;
}

.btn:hover {
  background-color: #134;
}

.navbar-collapse.show {
  display: block !important;
}

.navbar-nav {
  width: 100%;
}

.navbar-nav.flex-column .nav-item {
  width: 100%;
  text-align: center;
}

.navbar-nav.flex-column .nav-item .nav-link {
  padding: 1rem;
  width: 100%;
}

.nav-item {
  font-size: 0;
  padding: 0 10px;
  margin: 0 5px;
  transition: 1s;
  border-radius: 5px;
}

.nav-link {
  font-size: 16px;
}

.nav-item:hover {
  background-color: #999;
}

.nav-item:hover a {
  color: #333;
}

.full-height {
  height: calc(100vh - 56px); /* Full height minus navbar height */
}

.hidden-form {
  opacity: 0;
  pointer-events: none;
}
</style>